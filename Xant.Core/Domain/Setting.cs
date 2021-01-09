namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public class Setting : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets phone 1
        /// </summary>
        public string Phone1 { get; set; }
        /// <summary>
        /// Gets or sets phone 2
        /// </summary>
        public string Phone2 { get; set; }
        /// <summary>
        /// Gets or sets email 1
        /// </summary>
        public string Email1 { get; set; }
        /// <summary>
        /// Gets or sets email 2
        /// </summary>
        public string Email2 { get; set; }
        /// <summary>
        /// Gets or sets address 1
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets address 2
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// Gets or sets insagram profile link
        /// </summary>
        public string Instagram { get; set; }
        /// <summary>
        /// Gets or sets telegram profile link
        /// </summary>
        public string Telegram { get; set; }
        /// <summary>
        /// Gets or sets google plus profile link
        /// </summary>
        public string GooglePlus { get; set; }
        /// <summary>
        /// Gets or sets facebook profile link
        /// </summary>
        public string FaceBook { get; set; }
        /// <summary>
        /// Gets or sets linkedIn profile link
        /// </summary>
        public string LinkedIn { get; set; }
        /// <summary>
        /// Gets or sets youtube profile link
        /// </summary>
        public string Youtube { get; set; }
        /// <summary>
        /// Gets or sets apart profile link
        /// </summary>
        public string Aparat { get; set; }
        /// <summary>
        /// Gets or sets github profile link
        /// </summary>
        public string GitHub { get; set; }
    }
}
