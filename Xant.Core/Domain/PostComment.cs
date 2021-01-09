using System;
using System.Collections.Generic;

namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a post comment
    /// </summary>
    public class PostComment : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets post comment ParentId
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Gets or sets post comment parent
        /// </summary>
        public PostComment Parent { get; set; }
        /// <summary>
        /// Gets or sets post comment children
        /// </summary>
        public ICollection<PostComment> Children { get; set; }
        /// <summary>
        /// Gets or sets post comment UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets post comment User
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets post comment PostId
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// Gets or sets post comment Post
        /// </summary>
        public Post Post { get; set; }
        /// <summary>
        /// Gets or sets post comment user fullname
        /// </summary>
        public string UserFullName { get; set; }
        /// <summary>
        /// Gets or sets post comment user email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets post comment body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets post comment user id
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets post comments status
        /// </summary>
        public PostCommentStatus Status { get; set; }
        /// <summary>
        /// Gets or sets post comment creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        ///  Gets or sets post comment last edit date
        /// </summary>
        public DateTime LastEditDate { get; set; }
        /// <summary>
        /// Gets or sets post comment IsEdited
        /// </summary>
        public bool IsEdited { get; set; }
    }
}
