using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class QueryTests
    {
        [Fact]
        public async Task ShouldReturnEmployeesWhenMatchWithSqlOnly()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Query("SELECT * FROM Employee WHERE Address = 'Some address'");
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
        }

        [Fact]
        public async Task ShouldReturnEmployeesWhenMatchWithSqlAndParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Query("SELECT * FROM Employee WHERE Address = @Address", new { Address = "Some address" });
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
        }

        [Fact]
        public async Task ShouldNotReturnEmployeesWhenNoMatchWithSqlOnly()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Query("SELECT * FROM Employee WHERE Address = 'Wrong address'");
            Assert.Empty(value);
        }

        [Fact]
        public async Task ShouldNotReturnEmployeesWhenNoMatchWithSqlAndParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Query("SELECT * FROM Employee WHERE Address = @Address", new { Address = "Wrong address" });
            Assert.Empty(value);
        }
    }
}
