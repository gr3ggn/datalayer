using System;
using System.Collections.Generic;
using DataLayer.Factories;
using Models.Constants;

namespace DataLayer.Tests.Factories
{
    public class TestSqliteConnectionFactory : SqliteConnectionFactory
    {
        public TestSqliteConnectionFactory(Dictionary<string, string> connectionStrings) : base(connectionStrings)
        {
            _connectionStrings = new Dictionary<string, string>
            {
                { DatabaseNames.YourDatabase, "Data Source=:memory:;datetimeformat=CurrentCulture" }
            };
        }

        protected override void CreateDatabase()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Employee (Id INTEGER PRIMARY KEY AUTOINCREMENT, first_name VARCHAR(50), last_name VARCHAR(50), address VARCHAR(50), date_of_birth DATETIME, create_date DATETIME)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Test', 'Employee1', 'Some address', '01-Mar-1980', '{DateTime.Today}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Test', 'Employee2', 'Some address', '01-Mar-1981', '{DateTime.Today}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Test', 'Employee3', 'Different address', '01-Mar-1982', '{DateTime.Today}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Test', 'Employee4', 'Different address', '01-Mar-1983', '{DateTime.Today}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"INSERT INTO Employee (first_name, last_name, address, date_of_birth, create_date) VALUES ('Some', 'Employee5', null, '{DateTime.MinValue}', null)";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
