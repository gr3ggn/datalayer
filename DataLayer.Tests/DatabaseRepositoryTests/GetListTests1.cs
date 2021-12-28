using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetListTests1
    {
        [Fact]
        public async Task ShouldReturnEmployeesWhenMatch()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList(new { Address = "Some address" });
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
        }

        [Fact]
        public async Task ShouldNotReturnEmployeesWhenNoMatch()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList(new { Address = "Wrong address" });
            Assert.Empty(value);
        }
    }
}
