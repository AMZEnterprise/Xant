using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Extensions;
using Xant.MVC.Areas.Panel.Models;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Models.Constants;
using Xant.MVC.Services.EmailSender;
using Xant.MVC.Services.FileHandler;

namespace Xant.MVC.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = ConstantUserRoles.SuperAdmin + "," + ConstantUserRoles.Admin + "," + ConstantUserRoles.Writer)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IFileHandler _fileHandler;
        private readonly IWebHostEnvironment _env;

        public UsersController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailSender emailSender,
            IFileHandler fileHandler,
            IWebHostEnvironment env
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
            _fileHandler = fileHandler;
            _env = env;
        }

        [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> LoadUsersTable([FromBody] DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = _unitOfWork.UserRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(r =>
                    (r.FirstName != null && r.FirstName.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.LastName != null && r.LastName.ToUpper().Contains(searchBy.ToUpper())) ||
                    (r.Email != null && r.Email.Contains(searchBy)) ||
                    (r.CreateDate.ToString("F") != null && r.CreateDate.ToString("F").Contains(searchBy)) ||
                    (r.LastEditDate.ToString("F") != null && r.LastEditDate.ToString("F").Contains(searchBy))
                );
            }

            result = orderAscendingDirection ?
                result.OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc) :
                result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _unitOfWork.UserRepository.Count();

            var resultList = result
                .Skip(dtParameters.Start)
                .Take(dtParameters.Length)
                .ToList();

            return new JsonResult(new DatatableJsonResultModel()
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = _mapper
                    .Map<IEnumerable<User>, IEnumerable<UserIndexViewModel>>(resultList)
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _unitOfWork.UserRepository.FindById(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(currentUser, user.Id, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<User, UserFormViewModel>(user));
        }

        [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateFormViewModel userCreateFormViewModel)
        {
            if (userCreateFormViewModel.Role == UserRoleEnumViewModel.SuperAdmin)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<UserCreateFormViewModel, User>(userCreateFormViewModel);

                var addToIdentityResult = await _unitOfWork.UserRepository.Insert(user, userCreateFormViewModel.Password);

                if (!addToIdentityResult.Succeeded)
                {
                    if (addToIdentityResult.Errors.Any(c => c.Code == "DuplicateUserName"))
                    {
                        ModelState.AddModelError(nameof(userCreateFormViewModel.UserName),
                            ConstantMessages.IdentityDuplicateUserName);
                    }

                    if (addToIdentityResult.Errors.Any(c => c.Code.Contains("InvalidUserName")))
                    {
                        ModelState.AddModelError(nameof(userCreateFormViewModel.UserName), ConstantMessages.IdentityInvalidUserName);
                    }

                    if (addToIdentityResult.Errors.Any(c => c.Code.Contains("Email")))
                    {
                        ModelState.AddModelError(nameof(userCreateFormViewModel.Email), ConstantMessages.IdentityEmail);
                    }

                    if (addToIdentityResult.Errors.Any(c => c.Code.Contains("Password")))
                    {
                        ModelState.AddModelError(nameof(userCreateFormViewModel.Password), ConstantMessages.IdentityPassword);
                    }

                    return View(userCreateFormViewModel);
                }

                var addToRoleResult = await _unitOfWork.UserRepository.AddToRole(user, userCreateFormViewModel.Role.ToString());
                if (!addToRoleResult.Succeeded)
                {
                    ViewData["Error"] = ConstantMessages.IdentityAddToRoleResult;
                    return View(userCreateFormViewModel);
                }

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    await _fileHandler.UploadMedia(
                        files,
                        _env.WebRootPath,
                        user.FilesPathGuid.ToString(),
                        FileHandlerFolder.Profiles);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(userCreateFormViewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _unitOfWork.UserRepository.FindById(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(currentUser, user.Id, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<User, UserFormViewModel>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserFormViewModel userFormViewModel)
        {
            if (id != userFormViewModel.Id)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.FindById(userFormViewModel.Id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = await _unitOfWork.UserRepository.GetByClaimsPrincipal(HttpContext.User);

            if (!await _unitOfWork.UserRepository
                .IsUserAllowedForOperation(currentUser, user.Id, ConstantUserRoles.SuperAdmin))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(userFormViewModel.Password))
                {
                    var removePassResult = await _unitOfWork.UserRepository.RemovePassword(user);

                    if (!removePassResult.Succeeded)
                    {
                        ViewData["Error"] = ConstantMessages.IdentityError;
                        return View(userFormViewModel);
                    }

                    user.PasswordHash = _unitOfWork.UserRepository.HashPassword(user, userFormViewModel.Password);
                }

                user.UserName = userFormViewModel.UserName;
                user.FirstName = userFormViewModel.FirstName;
                user.LastName = userFormViewModel.LastName;
                user.Email = userFormViewModel.Email;
                user.Biography = userFormViewModel.Biography;
                user.IsActive = userFormViewModel.IsActive;

                var updateRoleResult = await _unitOfWork.UserRepository.UpdateRole(user, userFormViewModel.Role.ToString());
                if (!updateRoleResult.Succeeded)
                {
                    ViewData["Error"] = ConstantMessages.IdentityError;
                    return View(userFormViewModel);
                }

                var result = await _unitOfWork.UserRepository.Update(user);

                if (result.Succeeded)
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        //Delete old file
                        _fileHandler.DeleteMedia(
                            _env.WebRootPath,
                            user.FilesPathGuid.ToString(),
                            FileHandlerFolder.Profiles
                        );

                        //Upload new file
                        await _fileHandler.UploadMedia(
                            files,
                            _env.WebRootPath,
                            user.FilesPathGuid.ToString(),
                            FileHandlerFolder.Profiles);
                    }

                    await _unitOfWork.UserRepository.SignOut();

                    return RedirectToAction(nameof(Index));
                }

                ViewData["Error"] = ConstantMessages.IdentityError;
                return View(userFormViewModel);
            }

            return View(userFormViewModel);
        }

        [Authorize(Roles = ConstantUserRoles.SuperAdmin)]
        public async Task<JsonResult> ToggleActive(string id)
        {
            var user = await _unitOfWork.UserRepository.FindById(id);
            var isSuperAdmin = await _unitOfWork.UserRepository.IsInRole(user, ConstantUserRoles.SuperAdmin);

            if (user == null || isSuperAdmin)
            {
                return new JsonResult(BadRequest());
            }

            user.IsActive = !user.IsActive;
            await _unitOfWork.UserRepository.Update(user);

            return new JsonResult(Ok());
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginFormViewModel userLoginFormViewModel, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.FindByUserName(userLoginFormViewModel.Username);

                if (user != null)
                {
                    if (user.IsActive)
                    {
                        await _unitOfWork.UserRepository.SignOut();

                        var result = await _unitOfWork.UserRepository
                            .SignInByPassword(
                                user,
                                userLoginFormViewModel.Password,
                                userLoginFormViewModel.RememberMe,
                                false
                            );

                        if (result.Succeeded)
                        {
                            ViewBag.returnUrl = returnUrl;
                            return LocalRedirect(returnUrl ?? "/");
                        }

                        ModelState.AddModelError(nameof(userLoginFormViewModel.Password),
                            ConstantMessages.IdentityError);
                    }
                    else
                    {
                        ViewData["Error"] = ConstantMessages.IdentityAccountDisabled;
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(userLoginFormViewModel.Username),
                        ConstantMessages.IdentityAccountDisabled);
                }
            }

            return View(userLoginFormViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordFormViewModel userForgotPasswordFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.FindByEmail(userForgotPasswordFormViewModel.Email);

                if (user == null)
                {
                    ViewData["Error"] = ConstantMessages.IdentityNotRegisteredYet;
                }
                else
                {
                    var code = await _unitOfWork.UserRepository.GeneratePasswordResetToken(user);
                    var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                    var resetLink = $"<a href=\"{callbackUrl}\">link</a>";
                    var isEmailSent = _emailSender
                        .SendEmail(userForgotPasswordFormViewModel.Email,
                            ConstantMessages.VerificationEmailSubject,
                            string.Format(ConstantMessages.VerificationEmailText, resetLink));

                    if (isEmailSent)
                    {
                        return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    ViewData["Error"] = ConstantMessages.EmailFailedOnSending;
                }
            }

            return View(userForgotPasswordFormViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _unitOfWork.UserRepository.SignOut();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                ViewData["Error"] = ConstantMessages.IdentityPasswordVerifyCode;
            }

            return View(new UserResetPasswordFormViewModel() { Code = code });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserResetPasswordFormViewModel userResetPasswordFormViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.FindByEmail(userResetPasswordFormViewModel.Email);

                if (user == null)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                var result = await _unitOfWork.UserRepository
                    .ResetPassword(user, userResetPasswordFormViewModel.Code, userResetPasswordFormViewModel.Password);

                if (result.Succeeded)
                {
                    await _unitOfWork.UserRepository.SignIn(user, false);
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                ViewData["Error"] = ConstantMessages.IdentityUpdatePassword;
            }

            return View(userResetPasswordFormViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}