using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Models;

namespace DataLayer.Interfaces
{
    public interface IDatabaseRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters);

        Task<IEnumerable<T>> ExecuteStoredProc(string spName, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters);

        Task<bool> ExecuteStoredProcNonQuery(string spName, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<int?> Insert(T model);

        Task<int?> Insert(T model, IDbTransaction transaction, int? commandTimeout);

        Task<bool> BulkInsert(IEnumerable<T> list);

        Task<bool> Execute(string query);

        Task<bool> Execute(string query, object parameters);

        Task<bool> Execute(string query, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<IEnumerable<T>> Query(string query);

        Task<IEnumerable<T>> Query(string query, object parameters);

        Task<IEnumerable<T>> Query(string query, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<T> Get(int id);

        Task<T> Get(int id, IDbTransaction transaction, int? commandTimeout);

        Task<IEnumerable<T>> GetList(object parameters);

        Task<IEnumerable<T>> GetList(object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<IEnumerable<T>> GetList(string conditions, object parameters);

        Task<IEnumerable<T>> GetList(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields);

        Task<IEnumerable<T>> GetListPaged(int pageNumber, int rowsPerPage, string conditions, object parameters, string orderbyFields, IDbTransaction transaction, int? commandTimeout);

        Task<int> GetRowCount(string conditions, object parameters);

        Task<int> GetRowCount(string conditions, object parameters, IDbTransaction transaction, int? commandTimeout);

        Task<IEnumerable<T>> GetAll();

        Task<T> Update(int id, T model, List<string> changedFields);

        Task<T> Update(int id, T model, List<string> changedFields, IDbTransaction transaction, int? commandTimeout);

        Task<bool> Update(T model);

        Task<bool> Update(T model, IDbTransaction transaction, int? commandTimeout);
    }
}
