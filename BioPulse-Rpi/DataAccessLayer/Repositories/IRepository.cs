namespace DataAccessLayer.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id); // Retrieve a single entity by its ID
        Task<IEnumerable<T>> GetAllAsync(); // Retrieve all entities

        Task AddAsync(T entity); // Add a single entity
        

        Task UpdateAsync(T entity); // Update a single entity

        Task DeleteAsync(int id); // Delete an entity by its ID
    }
}
