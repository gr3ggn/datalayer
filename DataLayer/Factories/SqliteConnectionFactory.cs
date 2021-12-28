using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using DataLayer.Interfaces;
using Dapper;
using DapperExtensions.Sql;

namespace DataLayer.Factories
{
    public class SqliteConnectionFactory : IDatabaseConnectionFactory
    {
        protected Dictionary<string, string> _connectionStrings;
        protected IDbConnection _connection;

        public SqliteConnectionFactory(Dictionary<string, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;

            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
        }

        public IDbConnection GetNew()
        {
            return GetNew(null);
        }

        public IDbConnection GetNew(string database)
        {
            if (_connectionStrings == null || _connectionStrings.Count == 0)
                throw new ArgumentNullException($"No database connections have been defined in SqliteConnectionFactory");

            if (!string.IsNullOrWhiteSpace(database) && !_connectionStrings.ContainsKey(database))
                throw new ArgumentException($"A connection string for database {database} not found");

            // If no database name passed in then just return the first element in the dictionary
            var connString = string.IsNullOrWhiteSpace(database) ? _connectionStrings.ElementAt(0).Value : _connectionStrings[database];

            //Always return a new connection with new test data because every call in the DatabaseRepository class will dispose it
            _connection = new SQLiteConnection(connString);
            _connection.Open();
            CreateDatabase();

            return _connection;
        }

        protected virtual void CreateDatabase()
        {
            // This method can be overridden for test purposes
        }
    }
}
