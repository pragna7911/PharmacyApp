using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Wellgistics.Pharmacy.api.IRepositries
{
    public interface IRepository
    {
        Task<int> ExecuteScalarAsync(string storedProcedure, DbContext _context, params SqlParameter[] parameters);
        Task<List<T>> GetAllAsync<T>(string storedProcedure, DbContext _context, params SqlParameter[] parameters) where T : class;
        Task<T> GetAsync<T>(string storedProcedure, DbContext _context, params SqlParameter[] parameters) where T : class;
        Task<string> ExecuteStringAsync(string storedProcedure, DbContext _context, params SqlParameter[] parameters);
        Task<IEnumerable<T>> GetDataAsync<T>(DbContext _context) where T : class;
    }
}
