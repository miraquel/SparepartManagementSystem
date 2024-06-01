using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface
{
    public interface INumberSequenceService
    {
        Task<ServiceResponse> AddNumberSequence(NumberSequenceDto entity);
        Task<ServiceResponse> UpdateNumberSequence(NumberSequenceDto entity);
        Task<ServiceResponse> DeleteNumberSequence(int id);
        Task<ServiceResponse<NumberSequenceDto>> GetNumberSequenceById(int id);
        Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetAllNumberSequence();
        Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetNumberSequenceByParams(
            Dictionary<string, string> parameters);
    }
}
