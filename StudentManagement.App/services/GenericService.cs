using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagement.Domain.Errors;
using StudentManagement.Domain.Interface;

namespace StudentManagement.App.services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var items = await _repository.GetAllAsync();
                return ServiceResult<IEnumerable<T>>.AsSuccess(items);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<T>>.AsFailure(ex.Message);
            }
        }

        public async Task<ServiceResult<T>> GetByIdAsync(object id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ServiceResult<T>.AsFailure("Entity not found.");
                return ServiceResult<T>.AsSuccess(entity);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.AsFailure(ex.Message);
            }
        }

        public async Task<ServiceResult<T>> AddAsync(T entity)
        {
            try
            {
                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();
                return ServiceResult<T>.AsSuccess(entity);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.AsFailure(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> UpdateAsync(T entity)
        {
            try
            {
                _repository.Update(entity);
                await _repository.SaveChangesAsync();
                return ServiceResult<bool>.AsSuccess(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.AsFailure(ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(object id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ServiceResult<bool>.AsFailure("Entity not found.");

                _repository.Delete(entity);
                await _repository.SaveChangesAsync();
                return ServiceResult<bool>.AsSuccess(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.AsFailure(ex.Message);
            }
        }
    }


}