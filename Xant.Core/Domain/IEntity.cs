namespace Xant.Core.Domain
{
    /// <summary>
    /// Represents primary key for database
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        int Id { get; set; }
    }
}
