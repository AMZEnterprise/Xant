using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers.WebsiteFrontResolvers
{
    /// <summary>
    /// Auto mapper resolver for PostCommentViewModel UserFullName
    /// </summary>
    public class PostCommentViewModelUserFullName : IValueResolver<PostComment, PostCommentViewModel, string>
    {
        public string Resolve(PostComment source, PostCommentViewModel destination, string destMember, ResolutionContext context)
        {
            return source.User == null ? source.UserFullName : source.User.FirstName + " " + source.User.LastName;
        }
    }
}
