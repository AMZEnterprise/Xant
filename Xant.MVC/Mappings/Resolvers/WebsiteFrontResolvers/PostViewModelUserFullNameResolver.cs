using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers.WebsiteFrontResolvers
{
    /// <summary>
    /// Auto mapper resolver for PostViewModel UserFullName
    /// </summary>
    public class PostViewModelUserFullNameResolver : IValueResolver<Post, PostViewModel, string>
    {
        public string Resolve(Post source, PostViewModel destination, string destMember, ResolutionContext context)
        {
            return source.User.FirstName + " " + source.User.LastName;
        }
    }
}
