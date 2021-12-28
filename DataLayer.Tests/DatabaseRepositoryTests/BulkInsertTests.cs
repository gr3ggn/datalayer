using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class BulkInsertTests
    {
        [Fact]
        public async Task ShouldThrowNotImplementedException()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<NotImplementedException>(async () => await repo.BulkInsert(new List<Employee>()));
            Assert.Equal("BulkInsert has not been implemented", ex.Message);
        }
    }
}
