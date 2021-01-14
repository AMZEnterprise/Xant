using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers
{
    /// <summary>
    /// Auto mapper resolver for UserIndexViewModel UserFullName
    /// </summary>
    public class UserIndexViewModelUserFullNameResolver : IValueResolver<User, UserIndexViewModel, string>
    {
        public string Resolve(User source, UserIndexViewModel destination, string destMember, ResolutionContext context)
        {
            return source.FirstName + " " + source.LastName;
        }
    }
}
