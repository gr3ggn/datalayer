using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetRowCountTests
    {
        [Fact]
        public async Task ShouldReturnCountWhenMatchWithParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetRowCount("WHERE Address = @Address", new { Address = "Some address" });
            Assert.Equal(2, value);
        }

        [Fact]
        public async Task ShouldReturnCountWhenMatchWithoutParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetRowCount("WHERE Address = 'Some address'", null);
            Assert.Equal(2, value);
        }

        [Fact]
        public async Task ShouldReturnZeroWhenNoMatchWithParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetRowCount("WHERE Address = @Address", new { Address = "Wrong address" });
            Assert.Equal(0, value);
        }

        [Fact]
        public async Task ShouldReturnZeroWhenNoMatchWithoutParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetRowCount("WHERE Address = 'Wrong address'", null);
            Assert.Equal(0, value);
        }

        [Fact]
        public async Task ShouldReturnTableRowCountWhenNoParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetRowCount(null, null);
            Assert.Equal(5, value);
        }
    }
}
