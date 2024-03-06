using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IService<T> where T : class
{
    Task<ServiceResponse<T>> Add(T dto);
    Task<ServiceResponse<T>> Delete(int id);
    Task<ServiceResponse<IEnumerable<T>>> GetAll();
    Task<ServiceResponse<T>> GetById(int id);
    Task<ServiceResponse<IEnumerable<T>>> GetByParams(T dto);
    Task<ServiceResponse<T>> Update(T dto);
}