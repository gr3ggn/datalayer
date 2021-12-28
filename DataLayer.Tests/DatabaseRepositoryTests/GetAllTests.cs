using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class GetAllTests
    {
        [Fact]
        public async Task ShouldReturnAllRows()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.GetAll();
            Assert.NotEmpty(value);
            Assert.Equal(5, value.ToList().Count());
        }
    }
}
