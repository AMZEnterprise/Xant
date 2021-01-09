using System;
using System.Collections.Generic;

namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a post category
    /// </summary>
    public class PostCategory : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets post category title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets post category creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        ///  Gets or sets post category last edit date
        /// </summary>
        public DateTime LastEditDate { get; set; }
        /// <summary>
        /// Gets or sets post category type
        /// </summary>
        public PostCategoryType Type { get; set; }
        /// <summary>
        /// Get or sets post category posts
        /// </summary>
        public ICollection<Post> Posts { get; set; }
    }
}
