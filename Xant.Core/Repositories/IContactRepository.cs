using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents contact repository interface 
    /// </summary>
    public interface IContactRepository
    {
        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns>returns all contacts</returns>
        IQueryable<Contact> GetAll();
        /// <summary>
        /// Get a contact by id
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns>returns a specific contact or null</returns>
        Task<Contact> GetById(int id);
        /// <summary>
        /// Insert new contact
        /// </summary>
        /// <param name="contact">new contact</param>
        void Insert(Contact contact);
        /// <summary>
        /// Update existing contact
        /// </summary>
        /// <param name="contact">updated contact</param>
        void Update(Contact contact);
        /// <summary>
        /// Delete a contact by id
        /// </summary>
        /// <param name="id">contact id</param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// Count total contacts
        /// </summary>
        /// <returns>returns contacts count</returns>
        Task<int> Count();
    }
}
