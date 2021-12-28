using System;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class InsertTests
    {
        [Fact]
        public async Task ShouldInsertRecordAndReturnRecordCountPlusOneAsId()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var rowCount = await repo.GetRowCount(null, null);
            var value = await repo.Insert(new Employee() { FirstName = "Test", LastName = "Employee1", Address = "Some address", DateOfBirth = DateTime.Today.AddYears(-30), CreateDate = DateTime.Today });
            Assert.Equal(rowCount + 1, value);
        }
    }
}
