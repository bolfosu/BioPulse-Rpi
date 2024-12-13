using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

            // Log the database connection string for debugging purposes
            Console.WriteLine($"[GenericRepository] Database Connection String: {_context.Database.GetDbConnection().ConnectionString}");
        }

        public DbSet<T> GetDbSet() => _dbSet;

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            // Retrieve the existing entity from the database
            var entityId = (int)typeof(T).GetProperty("Id").GetValue(entity);
            var existingEntity = await _dbSet.FindAsync(entityId);

            if (existingEntity == null)
            {
                throw new Exception("Entity not found.");
            }

            // Copy over values from the incoming entity to the tracked entity
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Now just save changes. EF is already tracking existingEntity.
            await _context.SaveChangesAsync();
        }

        


        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} does not exist.");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}