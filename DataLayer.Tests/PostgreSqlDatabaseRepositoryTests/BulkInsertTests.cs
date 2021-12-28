using System;
using DataLayer.Factories;
using DataLayer.Repositories;
using DataLayer.Tests.Models;
using Xunit;

namespace DataLayer.Tests.PostgreSqlDatabaseRepositoryTests
{
    public class BulkInsertTests
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWhenConnectionFactoryIsWrongType()
        {
            // I can't test the reverse of this without hard coding the license details which I 
            // would prefer not to do
            var connFactory = new SqliteConnectionFactory(null);
            var ex = Assert.Throws<ArgumentException>(() => new PostgreSqlDatabaseRepository<Employee>(connFactory));
            Assert.Equal("PostgreSqlDatabaseRepository must be passed a PostgreSqlConnectionFactory in it's constructor.", ex.Message);
        }

        // DapperPlus library requires a paid subscription so I'm commenting this code out.
        // If you have a subscription then uncomment this and the associated tests
        //[Fact]
        //public void ShouldThrowApplicationExceptionWhenDapperPlusLicenseIsNotValid()
        //{
        //    // I can't test the reverse of this without hard coding the license details which I 
        //    // would prefer not to do
        //    var connFactory = new PostgreSqlConnectionFactory(null);
        //    var ex = Assert.Throws<ApplicationException>(() => new PostgreSqlDatabaseRepository<Employee>(connFactory));
        //    Assert.Equal("DapperPlus license is not valid. Ensure that the license is configured correctly.", ex.Message);
        //}
    }
}
