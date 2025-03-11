using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Interface;
using StudentManagementSystem.Infrastructure.Persistence;

namespace StudentManagement.Infra.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StudentDbContext1 _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(StudentDbContext1 context)
        {
            _context = context;
            _dbSet = context.Set<T>();  // automatically sets the appropriate DbSet<T>
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}