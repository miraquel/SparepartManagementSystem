using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IService<T> where T : class
{
    Task<ServiceResponse> Add(T dto);
    Task<ServiceResponse> Delete(int id);
    Task<ServiceResponse<IEnumerable<T>>> GetAll();
    Task<ServiceResponse<T>> GetById(int id);
    Task<ServiceResponse<IEnumerable<T>>> GetByParams(T dto);
    Task<ServiceResponse> Update(T dto);
    Task<ServiceResponse<int>> GetLastInsertedId();
}