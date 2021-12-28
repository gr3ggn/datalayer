using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using Models;
using Dapper;

namespace DataLayer.Repositories
{
    public abstract class DatabaseRepository<T> : IDatabaseRepository<T> where T : BaseModel
    {
        public DatabaseRepository(IDatabaseConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
            DatabaseName = GetDatabaseAttribute();
        }

        /// <summary>
        /// Gets or sets the DbConnectionFactory property.
        /// Allows sub classes to use the factory to get a database connection.
        /// </summary>
        protected IDatabaseConnectionFactory DbConnectionFactory { get; set; }

        protected string DatabaseName { get; private set; }

        /// <summary>
        /// Runs a stored procedure, and returns results.
        /// </summary>
        public virtual async Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters)
        {
            return await ExecuteStoredProc(spName, parameters, null, null);
        }

        /// <summary>
        /// Runs a stored procedure, and returns results.
        /// </summary>
        public virtual async Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.QueryAsync<T>(spName, parameters, transaction, commandTimeout, CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs a stored procedure, and returns no results.
        /// </summary>
        public virtual async Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters)
        {
            return await ExecuteStoredProcNonQuery(spName, parameters, null, null);
        }

        /// <summary>
        /// Runs a stored procedure, and returns no results.
        /// </summary>
        public virtual async Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                await conn.ExecuteAsync(spName, parameters, transaction, commandTimeout, CommandType.StoredProcedure).ConfigureAwait(false);
            }

            return true;
        }

        /// <summary>
        /// Inserts the given model
        /// </summary>
        public virtual async Task<int?> Insert(T model)
        {
            return await Insert(model, null, null);
        }

        /// <summary>
        /// Inserts the given model
        /// </summary>
        public virtual async Task<int?> Insert(T model, IDbTransaction transaction, int? commandTimeout)
        {
            int? id;
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                id = await conn.InsertAsync(model, transaction, commandTimeout).ConfigureAwait(false);
            }

            return id;
        }

        /// <summary>
        /// This must be implemented in a subclass
        /// </summary>
        public virtual Task<bool> BulkInsert(IEnumerable<T> list)
        {
            throw new NotImplementedException("BulkInsert has not been implemented");
        }

        /// <summary>
        /// Execute a query that does not return results.
        /// </summary>
        public virtual async Task<bool> Execute(string query)
        {
            return await Execute(query, null, null, null);
        }

        /// <summary>
        /// Execute a query that does not return results.
        /// </summary>
        public virtual async Task<bool> Execute(string query, object parameters)
        {
            return await Execute(query, parameters, null, null);
        }

        /// <summary>
        /// Execute a query that does not return results.
        /// </summary>
        public virtual async Task<bool> Execute(string query, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                var result = await conn.ExecuteAsync(query, parameters, transaction, commandTimeout).ConfigureAwait(false);
                return result > 0;
            }
        }

        /// <summary>
        /// Execute a query that returns results
        /// </summary>
        public virtual async Task<IEnumerable<T>> Query(string query)
        {
            return await Query(query, null, null, null);
        }

        /// <summary>
        /// Execute a query that returns results
        /// </summary>
        public virtual async Task<IEnumerable<T>> Query(string query, object parameters)
        {
            return await Query(query, parameters, null, null);
        }

        /// <summary>
        /// Execute a query that returns results
        /// </summary>
        public virtual async Task<IEnumerable<T>> Query(string query, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.QueryAsync<T>(query, parameters, transaction, commandTimeout).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get an row from the database for a particular id
        /// </summary>
        public virtual async Task<T> Get(int id)
        {
            return await Get(id, null, null);
        }

        /// <summary>
        /// Get an row from the database for a particular id
        /// </summary>
        public virtual async Task<T> Get(int id, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.GetAsync<T>(id, transaction, commandTimeout).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get a list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetList(object parameters)
        {
            return await GetList(parameters, null, null);
        }

        /// <summary>
        /// Get a list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetList(object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.GetListAsync<T>(parameters, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// Get a list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetList(string conditions, object parameters)
        {
            return await GetList(conditions, parameters, null, null);
        }

        /// <summary>
        /// Get a list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetList(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.GetListAsync<T>(conditions, parameters, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// Get a paged list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields)
        {
            return await GetListPaged(pageNumber, rowsPerPage, conditions, parameters, orderbyFields, null, null);
        }

        /// <summary>
        /// Get a paged list of objects matching the where condition
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.GetListPagedAsync<T>(pageNumber, rowsPerPage, conditions, orderbyFields, parameters, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// Get the number of rows for a where condition
        /// </summary>
        public virtual async Task<int> GetRowCount(string conditions, object parameters)
        {
            return await GetRowCount(conditions, parameters, null, null);
        }

        /// <summary>
        /// Get the number of rows for a where condition
        /// </summary>
        public virtual async Task<int> GetRowCount(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.RecordCountAsync<T>(conditions, parameters, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// Returns all rows from the table. Be careful using this method on large tables.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                return await conn.GetListAsync<T>().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Update the object by first retrieving the original from the database and only updating those columns in the changedFields list
        /// </summary>
        public virtual async Task<T> Update(int id, T model, List<string> changedFields)
        {
            return await Update(id, model, changedFields, null, null);
        }

        /// <summary>
        /// Update the object by first retrieving the original from the database and only updating those columns in the changedFields list
        /// </summary>
        public virtual async Task<T> Update(int id, T model, List<string> changedFields, IDbTransaction transaction, int? commandTimeout)
        {
            if (model == null)
                throw new ArgumentNullException("Model");

            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero");

            var dbRow = await Get(id);

            if (dbRow == null)
                throw new ArgumentException("Unable to update as the original record cannot be found");

            var updateRequired = false;
            foreach (var field in changedFields)
            {
                var prop = model.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                    throw new ArgumentException($"Model does not contain a property called {field}");

                if (HasKeyAttribute(prop))
                    throw new ArgumentException($"Not permitted to update the primary key - {field}");

                var newValue = prop.GetValue(model);

                if ((prop.PropertyType == typeof(DateTime)) && (((DateTime)newValue).Year == DateTime.MinValue.Year)) // Ignore - null value is converted to DateTime.Min
                    continue;

                // Get the old value before we update
                var oldValue = prop.GetValue(dbRow);

                if ((oldValue == null && newValue != null) || (oldValue != null && newValue == null))
                {
                    updateRequired = true;
                    prop.SetValue(dbRow, newValue);
                }
                else if (oldValue == null && newValue == null)
                {
                    //Do nothing as no change both null
                    continue;
                }
                else if (!oldValue.Equals(newValue))
                {
                    updateRequired = true;
                    prop.SetValue(dbRow, newValue);
                }
            }

            if (updateRequired)
            {
                var success = await Update(dbRow, transaction, commandTimeout);
                if (!success)
                    return null;
            }

            return dbRow;
        }

        /// <summary>
        /// This assumes a model is fully updated and ready to update all fields
        /// </summary>
        public virtual async Task<bool> Update(T model)
        {
            return await Update(model, null, null);
        }

        /// <summary>
        /// This assumes a model is fully updated and ready to update all fields
        /// </summary>
        public virtual async Task<bool> Update(T model, IDbTransaction transaction, int? commandTimeout)
        {
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                var result = await conn.UpdateAsync<T>(model, transaction, commandTimeout).ConfigureAwait(false);
                return result > 0;
            }
        }

        /// <summary>
        /// Get the table name for the model from the custom attribute
        /// </summary>
        protected string GetTableName()
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute(typeof(TableAttribute), true) as TableAttribute;
            var tableName = string.Empty;

            if (attr != null)
                tableName = attr.Name;

            if (string.IsNullOrWhiteSpace(tableName))
                throw new NullReferenceException(typeof(T).Name + " does not have a valid Table attribute");

            return tableName;
        }

        protected string GetDatabaseAttribute()
        {
            var attribute = typeof(T).GetCustomAttributes(typeof(DatabaseAttribute), true).FirstOrDefault() as DatabaseAttribute;
            if (attribute != null)
            {
                return attribute.DatabaseName;
            }

            return null;
        }

        protected DataTable GetDataTable(IEnumerable<T> list)
        {
            var properties = typeof(T).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType)
                   ?? info.PropertyType);

            foreach (var entity in list)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
        }

        protected bool HasKeyAttribute(PropertyInfo propInfo)
        {
            //Check if the property has any of the Key attribute
            var attribute = propInfo.GetCustomAttributes(typeof(Dapper.KeyAttribute), true).FirstOrDefault() as Dapper.KeyAttribute;
            if (attribute != null)
                return true;

            var attribute2 = propInfo.GetCustomAttributes(typeof(Dapper.Contrib.Extensions.KeyAttribute), true).FirstOrDefault() as Dapper.Contrib.Extensions.KeyAttribute;
            if (attribute2 != null)
                return true;

            var attribute3 = propInfo.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).FirstOrDefault() as System.ComponentModel.DataAnnotations.KeyAttribute;
            return attribute3 != null;
        }
    }
}
