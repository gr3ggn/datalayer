using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class UpdateTests1
    {
        [Fact]
        public async Task ShouldUpdateEmployeeWhenExists()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(new Employee() { Id = 1, Address = "New address", FirstName = "Test", LastName = "Employee 1" });
            Assert.True(value);
        }

        [Fact]
        public async Task ShouldNotUpdateEmployeeWhenNotExists()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(new Employee() { Id = 999, Address = "New address", FirstName = "Test", LastName = "Employee 1" });
            Assert.False(value);
        }
    }
}
