using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xant.Core.Domain;

namespace Xant.Tests.Mocks
{
    /// <summary>
    /// Fake sign-in manager implementation
    /// </summary>
    public class FakeSignInManger : SignInManager<User>
    {
        public FakeSignInManger()
            : base(new FakeUserManager(),
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new DefaultUserConfirmation<User>())
        { }
    }
}
