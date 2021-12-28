using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetTests
    {
        [Fact]
        public async Task ShouldReturnEmployeeWhenIdExists()
        {
            var testId = 2;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Get(testId);
            Assert.NotNull(value);
            Assert.Equal(testId, value.Id);
        }

        [Fact]
        public async Task ShouldNotReturnEmployeeWhenIdDoesNotExists()
        {
            var testId = 999;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Get(testId);
            Assert.Null(value);
        }
    }
}
