using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Repositories;
using DataLayer.Tests.Factories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.DatabaseRepositoryTests
{
    public class UpdateTests2
    {
        [Fact]
        public async Task ShouldThrowExceptionWhenModelIsNull()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await repo.Update(1, null, new List<string>()));
            Assert.Equal("Value cannot be null. (Parameter 'Model')", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenIdIsZero()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Update(0, new Employee(), new List<string>()));
            Assert.Equal("Id must be greater than zero", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenEmployeeNotFound()
        {
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Update(999, new Employee(), new List<string>()));
            Assert.Equal("Unable to update as the original record cannot be found", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenPropertyNotExists()
        {
            var propertyName = "InvalidProperty";
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Update(1, new Employee(), new List<string>() { propertyName }));
            Assert.Equal($"Model does not contain a property called {propertyName}", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenPropertyHasKeyAttribute()
        {
            var propertyName = "Id";
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Update(1, new Employee(), new List<string>() { propertyName }));
            Assert.Equal($"Not permitted to update the primary key - {propertyName}", ex.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenStringValueHasNotChanged()
        {
            var id = 1;
            var address = "Some address";
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(id, new Employee() { Address = address }, new List<string>() { "Address" });
            Assert.Equal(address, value.Address);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenNullValueHasNotChanged()
        {
            var id = 5;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(id, new Employee() { Address = null }, new List<string>() { "Address" });
            Assert.Null(value.Address);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenDateTimeValueHasNotChanged()
        {
            var id = 1;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(id, new Employee() { CreateDate = DateTime.Today }, new List<string>() { "CreateDate" });
            Assert.Equal(DateTime.Today, value.CreateDate);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenDateTimeValueIsSetToMinDate()
        {
            var id = 1;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var employee = await repo.Get(id);
            var value = await repo.Update(id, new Employee() { DateOfBirth = DateTime.MinValue }, new List<string>() { "DateOfBirth" });
            Assert.Equal(employee.DateOfBirth, value.DateOfBirth);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task ShouldUpdateWhenValueChanges()
        {
            var id = 1;
            var address = "New address";
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(id, new Employee() { Address = address }, new List<string>() { "Address" });
            Assert.Equal(address, value.Address);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public async Task ShouldUpdateWhenValueChangesToNull()
        {
            var id = 1;
            var connFactory = new TestSqliteConnectionFactory(null);
            var repo = new SqliteDatabaseRepository<Employee>(connFactory);
            var value = await repo.Update(id, new Employee() { Address = null }, new List<string>() { "Address" });
            Assert.Null(value.Address);
            Assert.Equal(id, value.Id);
        }
    }
}
