﻿using System;

namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a contact form entry
    /// </summary>
    public class Contact : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets user fullname
        /// </summary>
        public string UserFullName { get; set; }
        /// <summary>
        /// Gets or sets user email address/ phone number
        /// </summary>
        public string EmailOrPhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets contact subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets contact body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets user ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets contact creation date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets contact last edit date
        /// </summary>
        public DateTime LastEditDate { get; set; }
    }
}