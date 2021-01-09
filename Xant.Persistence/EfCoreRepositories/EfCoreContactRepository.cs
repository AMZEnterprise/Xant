using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Core.Repositories;

namespace Xant.Persistence.EfCoreRepositories
{
    public class EfCoreContactRepository : IContactRepository
    {
        private readonly IApplicationDbContext _context;

        public EfCoreContactRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Contact> GetAll()
        {
            return _context.Contacts.AsQueryable();
        }

        public async Task<Contact> GetById(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public void Insert(Contact contact)
        {
            contact.CreateDate = DateTime.Now;

            _context.Contacts.Add(contact);
        }

        public void Update(Contact contact)
        {
            _context.Contacts.Update(contact);
        }

        public async Task Delete(int id)
        {
            var contact = await GetById(id);
            if (contact != null)
                _context.Contacts.Remove(contact);
        }

        public async Task<int> Count()
        {
            return await _context.Contacts.CountAsync();
        }
    }
}
