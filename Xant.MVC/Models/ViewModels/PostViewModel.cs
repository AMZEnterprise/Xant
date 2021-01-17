using System;
using System.Collections.Generic;
using Xant.Core.Domain;

namespace Xant.MVC.Models.ViewModels
{
    public class PostViewModel : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public string Tags { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserFilePath { get; set; }
        public string UserBiography { get; set; }
        public Guid FilesPathGuid { get; set; }
        public bool IsCommentsOn { get; set; }
        public string FilePath { get; set; }
        public PostCategoryViewModel PostCategoryViewModel { get; set; }
        public PostCommentFormViewModel PostCommentFormViewModel { get; set; }
        public IEnumerable<PostCommentViewModel> PostCommentViewModels { get; set; }
    }
}
