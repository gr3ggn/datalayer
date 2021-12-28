using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetListPagedTests
    {
        [Fact]
        public async Task ShouldReturnEmployeesFirstPageOrderAscending()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetListPaged(1, 2, "WHERE FirstName = @FirstName", new { FirstName = "Test" }, "Id");
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
            Assert.Equal(1, value.ToList()[0].Id);
        }

        [Fact]
        public async Task ShouldReturnEmployeesSecondPageOrderAscending()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetListPaged(2, 2, "WHERE FirstName = @FirstName", new { FirstName = "Test" }, "Id");
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
            Assert.Equal(3, value.ToList()[0].Id);
        }

        [Fact]
        public async Task ShouldReturnEmployeesFirstPageOrderDescending()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetListPaged(1, 2, "WHERE FirstName = @FirstName", new { FirstName = "Test" }, "Id DESC");
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
            Assert.Equal(4, value.ToList()[0].Id);
        }

        [Fact]
        public async Task ShouldReturnEmployeesSecondPageOrderDescending()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetListPaged(2, 2, "WHERE FirstName = @FirstName", new { FirstName = "Test" }, "Id DESC");
            Assert.NotEmpty(value);
            Assert.Equal(2, value.ToList().Count());
            Assert.Equal(2, value.ToList()[0].Id);
        }

        [Fact]
        public async Task ShouldReturnEmptyListWhenConditionDoesNotExist()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetListPaged(1, 2, "WHERE FirstName = @FirstName", new { FirstName = "Somebody" }, "Id");
            Assert.Empty(value);
        }
    }
}
