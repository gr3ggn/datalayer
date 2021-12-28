using System;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class ExecuteTests
    {
        [Fact]
        public async Task ShouldExecuteWithSqlOnly()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Execute($"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Test', 'Employee1', 'Some address', '01-Mar-1980', '{DateTime.Today}')");
            Assert.True(value);
        }

        [Fact]
        public async Task ShouldExecuteWithSqlAndParameters()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Execute($"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES (@FirstName, @LastName, @Address, @DateOfBirth, @CreateDate)", 
                new { FirstName = "Test", LastName = "Employee1", Address = "Some address", DateOfBirth = "01-Mar-1990", CreateDate = DateTime.Today });
            Assert.True(value);
        }
    }
}
