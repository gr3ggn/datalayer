using System.Data;

namespace DataLayer.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetNew();

        IDbConnection GetNew(string database);
    }
}
