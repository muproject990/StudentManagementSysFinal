using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        // Retrieve all instances of T
        Task<IEnumerable<T>> GetAllAsync();

        // Retrieve an instance of T by its primary key (object id in our example)
        Task<T> GetByIdAsync(object id);

        // Add a new instance of T
        Task AddAsync(T entity);

        // Update an existing instance of T
        void Update(T entity);

        // Delete an instance of T
        void Delete(T entity);

        // Save changes to the underlying data store
        Task SaveChangesAsync();
    }
}