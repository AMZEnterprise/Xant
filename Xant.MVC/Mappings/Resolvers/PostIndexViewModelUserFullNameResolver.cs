using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers
{
    /// <summary>
    /// Auto mapper resolver for PostIndexViewModel UserFullName
    /// </summary>
    public class PostIndexViewModelUserFullNameResolver : IValueResolver<Post, PostIndexViewModel, string>
    {
        public string Resolve(Post source, PostIndexViewModel destination, string destMember, ResolutionContext context)
        {
            return source.User?.FirstName + " " + source.User?.LastName;
        }
    }
}
