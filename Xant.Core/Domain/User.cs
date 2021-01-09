using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a user 
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets user first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets user last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets user biography
        /// </summary>
        public string Biography { get; set; }
        /// <summary>
        /// Gets or sets user IsActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets user FilesPathGuid
        /// </summary>
        public Guid FilesPathGuid { get; set; }
        /// <summary>
        /// Gets or sets user creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets user last edit date
        /// </summary>
        public DateTime LastEditDate { get; set; }
        /// <summary>
        /// Gets or sets user posts
        /// </summary>
        public ICollection<Post> Posts { get; set; }
        /// <summary>
        /// Gets or sets user post comments
        /// </summary>
        public ICollection<PostComment> PostComments { get; set; }
    }
}
