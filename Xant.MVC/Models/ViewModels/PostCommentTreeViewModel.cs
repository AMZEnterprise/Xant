using System.Collections.Generic;

namespace Xant.MVC.Models.ViewModels
{
    public class PostCommentTreeViewModel
    {
        public int? CommentSeed { get; set; }
        public IEnumerable<PostCommentViewModel> PostCommentViewModels { get; set; }
    }
}
