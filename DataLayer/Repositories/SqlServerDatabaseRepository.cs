using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataLayer.Factories;
using DataLayer.Interfaces;
using Models;

namespace DataLayer.Repositories
{
    public class SqlServerDatabaseRepository<T> : DatabaseRepository<T> where T : BaseModel
    {
        public SqlServerDatabaseRepository(IDatabaseConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            if (dbConnectionFactory.GetType() != typeof(SqlServerConnectionFactory))
            {
                throw new ArgumentException("SqlServerDatabaseRepository must be passed a SqlServerConnectionFactory in it's constructor");
            }
        }

        public override async Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters)
        {
            return await ExceptionRetryHelper(() => base.ExecuteStoredProc(spName, parameters, null, null));
        }

        public override async Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.ExecuteStoredProc(spName, parameters, transaction, commandTimeout));
        }

        public override async Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters)
        {
            return await ExceptionRetryHelper(() => base.ExecuteStoredProcNonQuery(spName, parameters, null, null));
        }

        public override async Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.ExecuteStoredProcNonQuery(spName, parameters, transaction, commandTimeout));
        }

        public override async Task<int?> Insert(T model)
        {
            return await ExceptionRetryHelper(() => base.Insert(model));
        }

        public override async Task<int?> Insert(T model, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Insert(model, transaction, commandTimeout));
        }

        /// <summary>
        /// Use the SqlBulkCopy function on the SqlConnection to insert bulk data
        /// </summary>
        public override async Task<bool> BulkInsert(IEnumerable<T> list)
        {
            var dt = GetDataTable(list);
            using (var conn = DbConnectionFactory.GetNew(DatabaseName))
            {
                using (var bulkCopy = new SqlBulkCopy((SqlConnection)conn))
                {
                    bulkCopy.DestinationTableName = GetTableName();
                    await bulkCopy.WriteToServerAsync(dt).ConfigureAwait(false);
                }
            }

            return true;
        }

        public override async Task<bool> Execute(string query)
        {
            return await ExceptionRetryHelper(() => base.Execute(query));
        }

        public override async Task<bool> Execute(string query, object parameters)
        {
            return await ExceptionRetryHelper(() => base.Execute(query, parameters));
        }

        public override async Task<bool> Execute(string query, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Execute(query, parameters, transaction, commandTimeout));
        }

        public override async Task<IEnumerable<T>> Query(string query)
        {
            return await ExceptionRetryHelper(() => base.Query(query));
        }

        public override async Task<IEnumerable<T>> Query(string query, object parameters)
        {
            return await ExceptionRetryHelper(() => base.Query(query, parameters));
        }

        public override async Task<IEnumerable<T>> Query(string query, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Query(query, parameters, transaction, commandTimeout));
        }

        public override async Task<T> Get(int id)
        {
            return await ExceptionRetryHelper(() => base.Get(id));
        }

        public override async Task<T> Get(int id, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Get(id, transaction, commandTimeout));
        }

        public override async Task<IEnumerable<T>> GetList(object parameters)
        {
            return await ExceptionRetryHelper(() => base.GetList(parameters));
        }

        public override async Task<IEnumerable<T>> GetList(object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.GetList(parameters, transaction, commandTimeout));
        }

        public override async Task<IEnumerable<T>> GetList(string conditions, object parameters)
        {
            return await ExceptionRetryHelper(() => base.GetList(conditions, parameters));
        }

        public override async Task<IEnumerable<T>> GetList(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.GetList(conditions, parameters, transaction, commandTimeout));
        }

        public override async Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields)
        {
            return await ExceptionRetryHelper(() => base.GetListPaged(pageNumber, rowsPerPage, conditions, parameters, orderbyFields));
        }

        public override async Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.GetListPaged(pageNumber, rowsPerPage, conditions, parameters, orderbyFields, transaction, commandTimeout));
        }

        public override async Task<int> GetRowCount(string conditions, object parameters)
        {
            return await ExceptionRetryHelper(() => base.GetRowCount(conditions, parameters));
        }

        public override async Task<int> GetRowCount(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.GetRowCount(conditions, parameters, transaction, commandTimeout));
        }

        public override async Task<IEnumerable<T>> GetAll()
        {
            return await ExceptionRetryHelper(() => base.GetAll());
        }

        public override async Task<T> Update(int id, T model, List<string> changedFields)
        {
            return await ExceptionRetryHelper(() => base.Update(id, model, changedFields));
        }

        public override async Task<T> Update(int id, T model, List<string> changedFields, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Update(id, model, changedFields, transaction, commandTimeout));
        }

        public override async Task<bool> Update(T model)
        {
            return await ExceptionRetryHelper(() => base.Update(model));
        }

        public override async Task<bool> Update(T model, IDbTransaction transaction, int? commandTimeout)
        {
            return await ExceptionRetryHelper(() => base.Update(model, transaction, commandTimeout));
        }

        /// <summary>
        /// Method to retry a database call if an exception occurs. Currently we only retry deadlocks and timeouts but we could add other exceptions if needed.
        /// </summary>
        protected T ExceptionRetryHelper<T>(Func<T> repositoryMethod)
        {
            int retryCount = 0;
            int maxRetries = 3;

            while (retryCount < maxRetries)
            {
                try
                {
                    return repositoryMethod();
                }
                catch (Exception ex)
                {
                    Exception innerEx = ex;
                    bool retryRequired = false;

                    while (innerEx != null)
                    {
                        if (innerEx.GetType().Equals(typeof(SqlException)))
                        {
                            // Look for 1205 (deadlock) or -2 (timeout) and if found retry the command
                            var sqlEx = (SqlException)innerEx;
                            if (sqlEx.Number == 1205 || sqlEx.Number == -2)
                            {
                                retryRequired = true;
                                retryCount++;
                                break;
                            }
                        }

                        innerEx = ex.InnerException;
                    }

                    if (!retryRequired || retryCount >= maxRetries)
                        throw;
                }
            }

            return default;  // This should never execute but compiler requires a return here
        }
    }
}
