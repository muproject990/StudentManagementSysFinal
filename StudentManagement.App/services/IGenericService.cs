using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.Domain.Errors;

namespace StudentManagement.App.services
{
    public interface IGenericService<T> where T : class
    {
        Task<ServiceResult<IEnumerable<T>>> GetAllAsync();
        Task<ServiceResult<T>> GetByIdAsync(object id);
        Task<ServiceResult<T>> AddAsync(T entity);
        Task<ServiceResult<bool>> UpdateAsync(T entity);
        Task<ServiceResult<bool>> DeleteAsync(object id);
    }
}