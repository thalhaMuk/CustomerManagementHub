using CustomerManagementHub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerManagementHub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                return await _context.FindAsync<T>(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding entity");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all entities");
                throw new Exception("An error occurred while processing your request");

            }
        }

        public async Task CreateAsync(T entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating entity");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating entity");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var entity = await _context.FindAsync<T>(id);
                if (entity != null)
                {
                    _context.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting entity");
                throw new Exception("An error occurred while processing your request");
            }
        }
    }
}
