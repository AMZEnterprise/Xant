using System;
using System.Collections.Generic;

namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a post 
    /// </summary>
    public class Post : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets post title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets post body
        /// </summary>
        public string Body{ get; set; }
        /// <summary>
        /// Gets or sets post creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets post last edit date
        /// </summary>
        public DateTime LastEditDate { get; set; }
        /// <summary>
        /// Gets or sets post tags
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Gets or sets PostCategoryId 
        /// </summary>
        public int? PostCategoryId { get; set; }
        /// <summary>
        /// Gets or sets PostCategory
        /// </summary>
        public PostCategory PostCategory { get; set; }
        /// <summary>
        /// Gets or sets post UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets post user
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets post IsCommentsOn
        /// </summary>
        public bool IsCommentsOn { get; set; }
        /// <summary>
        /// Gets or sets post FilesPathGuid
        /// </summary>
        public Guid FilesPathGuid { get; set; }
        /// <summary>
        /// Gets or sets post comments
        /// </summary>
        public ICollection<PostComment> PostComments { get; set; }
    }
}
