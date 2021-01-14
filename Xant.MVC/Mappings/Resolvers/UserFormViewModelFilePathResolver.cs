using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;
using Xant.MVC.Services.FileHandler;

namespace Xant.MVC.Mappings.Resolvers
{
    /// <summary>
    /// Auto mapper resolver for UserFormViewModel FilePath
    /// </summary>
    public class UserFormViewModelFilePathResolver : IValueResolver<User, UserFormViewModel, string>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileHandler _fileHandler;

        public UserFormViewModelFilePathResolver(IWebHostEnvironment env, IFileHandler fileHandler)
        {
            _env = env;
            _fileHandler = fileHandler;
        }
        public string Resolve(User source, UserFormViewModel destination, string destMember, ResolutionContext context)
        {
            return _fileHandler.GetFileSource(
                _env.WebRootPath,
                source.FilesPathGuid.ToString(),
                FileHandlerFolder.Profiles);
        }
    }
}
