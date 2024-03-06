using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Service.Interface;

public interface IActiveDirectoryService
{
    IEnumerable<User> GetUsersFromActiveDirectory();
}