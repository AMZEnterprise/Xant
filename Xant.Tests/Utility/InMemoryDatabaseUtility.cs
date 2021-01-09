using Microsoft.EntityFrameworkCore;
using System;
using Xant.Persistence;

namespace Xant.Tests.Utility
{
    /// <summary>
    /// Ef core in-memory database utility for test purposes
    /// </summary>
    public static class InMemoryDatabaseUtility
    {
        //Get in-memory database default options
        private static DbContextOptions<ApplicationDbContext> GetDatabaseOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        /// <summary>
        /// Get in-memory database context with default options
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext GetInMemoryDatabaseContext()
        {
            return new ApplicationDbContext(GetDatabaseOptions());
        }
    }
}
