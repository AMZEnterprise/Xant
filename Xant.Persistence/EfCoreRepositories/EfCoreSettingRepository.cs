using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Core.Repositories;

namespace Xant.Persistence.EfCoreRepositories
{
    public class EfCoreSettingRepository : ISettingRepository
    {
        private readonly IApplicationDbContext _context;
        public EfCoreSettingRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Setting> Get()
        {
            return await _context.Settings.FirstOrDefaultAsync();
        }

        public void Update(Setting setting)
        {
            _context.Settings.Update(setting);
        }
    }
}
