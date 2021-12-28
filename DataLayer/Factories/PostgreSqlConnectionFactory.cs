using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataLayer.Interfaces;
using Npgsql;

namespace DataLayer.Factories
{
    public class PostgreSqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly Dictionary<string, string> _connectionStrings;

        public PostgreSqlConnectionFactory(Dictionary<string, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public IDbConnection GetNew()
        {
            return GetNew(null);
        }

        public IDbConnection GetNew(string database)
        {
            if (_connectionStrings == null || _connectionStrings.Count == 0)
                throw new ArgumentNullException($"No database connections have been defined in PostgreSqlConnectionFactory");

            if (!string.IsNullOrWhiteSpace(database) && !_connectionStrings.ContainsKey(database))
                throw new ArgumentException($"A connection string for database {database} not found");

            // If no database name passed in then just return the first element in the dictionary
            var connString = string.IsNullOrWhiteSpace(database) ? _connectionStrings.ElementAt(0).Value : _connectionStrings[database];

            var conn = new NpgsqlConnection(connString);
            conn.Open();

            return conn;
        }
    }
}
