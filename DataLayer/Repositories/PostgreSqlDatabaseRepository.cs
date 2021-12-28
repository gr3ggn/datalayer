using System;
using DataLayer.Factories;
using DataLayer.Interfaces;
using Models;
using Dapper;
using DapperExtensions.Sql;
//using Z.Dapper.Plus;

namespace DataLayer.Repositories
{
    public class PostgreSqlDatabaseRepository<T> : DatabaseRepository<T> where T : BaseModel
    {
        public PostgreSqlDatabaseRepository(IDatabaseConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            if (dbConnectionFactory.GetType() != typeof(PostgreSqlConnectionFactory))
            {
                throw new ArgumentException("PostgreSqlDatabaseRepository must be passed a PostgreSqlConnectionFactory in it's constructor.");
            }

            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            // License must be set in calling application
            // DapperPlus library requires a paid subscription so I'm commenting this code out.
            // If you have a subscription then uncomment this and the associated tests
            //if (!DapperPlusManager.ValidateLicense())
            //    throw new ApplicationException("DapperPlus license is not valid. Ensure that the license is configured correctly.");
        }

        /// <summary>
        /// Use DapperPlus third party library to insert bulk data.
        /// DapperPlus library requires a paid subscription so I'm commenting this code out.
        /// If you have a subscription then uncomment this and the associated tests
        /// </summary>
        //public override async Task<bool> BulkInsert(IEnumerable<T> list)
        //{
        //    using (var conn = DbConnectionFactory.GetNew(DatabaseName))
        //    {
        //        conn.BulkInsert(list);
        //    }

        //    return await Task.FromResult(true);
        //}
    }
}
