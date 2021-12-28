using System;
using DataLayer.Factories;
using DataLayer.Interfaces;
using Models;

namespace DataLayer.Repositories
{
    public class SqliteDatabaseRepository<T> : DatabaseRepository<T> where T : BaseModel
    {
        public SqliteDatabaseRepository(IDatabaseConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            if (!(dbConnectionFactory is SqliteConnectionFactory))
            {
                throw new ArgumentException("SqliteDatabaseRepository must be passed a SqliteConnectionFactory in it's constructor");
            }
        }
    }
}
