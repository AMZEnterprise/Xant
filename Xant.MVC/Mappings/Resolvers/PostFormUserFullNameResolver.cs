using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers
{
    /// <summary>
    /// Auto mapper resolver for PostFormViewModel UserFullName
    /// </summary>
    public class PostFormUserFullNameResolver : IValueResolver<Post, PostFormViewModel, string>
    {
        public string Resolve(Post source, PostFormViewModel destination, string destMember, ResolutionContext context)
        {
            return source.User?.FirstName + " " + source.User?.LastName;
        }
    }
}
