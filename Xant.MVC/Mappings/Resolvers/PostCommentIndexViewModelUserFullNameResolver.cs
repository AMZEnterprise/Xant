using AutoMapper;
using Xant.Core.Domain;
using Xant.MVC.Areas.Panel.Models.ViewModels;

namespace Xant.MVC.Mappings.Resolvers
{
    /// <summary>
    /// Auto mapper resolver for PostCommentIndexViewModel UserFullName
    /// </summary>
    public class PostCommentIndexViewModelUserFullNameResolver : IValueResolver<PostComment, PostCommentIndexViewModel, string>
    {
        public string Resolve(PostComment source, PostCommentIndexViewModel destination, string destMember, ResolutionContext context)
        {
            return source.User == null ? source.UserFullName : source.User.FirstName + " " + source.User.LastName;
        }
    }
}
