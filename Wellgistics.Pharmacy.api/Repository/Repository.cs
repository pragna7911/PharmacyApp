using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Wellgistics.Pharmacy.api.IRepositries;
namespace Wellgistics.Pharmacy.api.Repository
{
    public class Repository : IRepository
    {
        public Repository()
        {
            
        }

        public async Task<int> ExecuteScalarAsync(string storedProcedure, DbContext _context, params SqlParameter[] parameters)
        {
            try
            {
                return await _context.Database.ExecuteSqlRawAsync(storedProcedure, parameters);
            }
            catch
            {
                throw;
            }

        }

        public async Task<List<T>> GetAllAsync<T>(string storedProcedure, DbContext _context, params SqlParameter[] parameters) where T : class
        {
            try
            {
                return await _context.Set<T>().FromSqlRaw(storedProcedure, parameters).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string storedProcedure, DbContext _context, params SqlParameter[] parameters) where T : class
        {
            try
            {
                var result = await _context.Set<T>().FromSqlRaw(storedProcedure, parameters).ToListAsync();
                return result!.FirstOrDefault();

            }
            catch
            {
                throw;
            }

        }

        public async Task<string> ExecuteStringAsync(string storedProcedure, DbContext _context, params SqlParameter[] parameters)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(storedProcedure, parameters);
                return result.ToString();
            }
            catch
            {
                throw;
            }

        }
        public async Task<IEnumerable<T>> GetDataAsync<T>(DbContext _context) where T : class
        {
            // Use reflection to access DbSet for the specific model type
            var dbSet = _context.Set<T>();  // _context.Set<T>() returns the DbSet<T> dynamically

            // Fetch data asynchronously
            return await dbSet.ToListAsync();
        }
    }
}
