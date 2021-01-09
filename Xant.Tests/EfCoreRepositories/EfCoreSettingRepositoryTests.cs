using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Persistence;
using Xant.Persistence.EfCoreRepositories;
using Xant.Tests.Utility;

namespace Xant.Tests.EfCoreRepositories
{
    [TestFixture]
    public class EfCoreSettingRepositoryTests
    {
        private List<Setting> _data;
        private ApplicationDbContext _context;
        private EfCoreSettingRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<Setting>()
            {
                new Setting()
                {
                    Id = 1
                },
                new Setting()
                {
                    Id = 2
                },
                new Setting()
                {
                    Id = 3
                }
            };

            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.Settings.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCoreSettingRepository(_context);
        }

        [Test]
        public async Task Get_ThereAreMoreThanOneSetting_ReturnFirstSettingOrNull()
        {
            var result = await _repository.Get();
            result.Should().BeSameAs(_data.FirstOrDefault());
        }
    }
}
