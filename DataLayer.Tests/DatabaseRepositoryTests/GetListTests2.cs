using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetListTests2
    {
        [Fact]
        public async Task ShouldReturnEmployeesWhenMatchWithParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList("WHERE Address = @Address", new { Address = "Some address" });
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
        }

        [Fact]
        public async Task ShouldReturnEmployeesWhenMatchWithoutParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList("WHERE Address = 'Some address'", null);
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
        }

        [Fact]
        public async Task ShouldNotReturnEmployeesWhenNoMatchWithParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList("WHERE Address = @Address", new { Address = "Wrong address" });
            Assert.Empty(value);
        }

        [Fact]
        public async Task ShouldNotReturnEmployeesWhenNoMatchWithoutParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetList("WHERE Address = 'Wrong address'", null);
            Assert.Empty(value);
        }
    }
}
