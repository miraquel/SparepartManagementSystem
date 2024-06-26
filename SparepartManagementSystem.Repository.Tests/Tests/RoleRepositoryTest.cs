using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

public class RoleRepositoryTest
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RoleRepositoryTest()
    {
        _serviceScopeFactory = _serviceCollectionHelper.GetRequiredService<IServiceScopeFactory>();
    }
    
    [Fact]
    public async Task Add_ShouldAddRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
        
            // Act
            await unitOfWork.RoleRepository.Add(role);
            
            // Assert
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
            var result = await unitOfWork.RoleRepository.GetById(id);
            Assert.Equal(role.RoleId, result.RoleId);
            Assert.Equal(role.RoleName, result.RoleName);
            Assert.Equal(role.Description, result.Description);
            Assert.Equal(role.CreatedBy, result.CreatedBy);
            Assert.Equal(role.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(role.ModifiedBy, result.ModifiedBy);
            Assert.Equal(role.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
        
            // Act
            await unitOfWork.RoleRepository.Delete(id);
            
            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.RoleRepository.GetById(id));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllRoles()
    {
        // Setup
        Role[] roles = [];
        
        try
        {
            // Arrange
            var tasks = new List<Task<Role>>
            {
                AddRole(), 
                AddRole()
            };
            roles = await Task.WhenAll(tasks);
        
            // Act
            using var getAllScope = _serviceScopeFactory.CreateScope();
            var getAllService = getAllScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var result = await getAllService.RoleRepository.GetAll();
            
            // Assert
            var resultArray = result as Role[] ?? result.ToArray();
            foreach (var role in roles)
            {
                var resultRole = resultArray.FirstOrDefault(r => r.RoleId == role.RoleId);
                Assert.NotNull(resultRole);
                Assert.Equal(role.RoleId, resultRole.RoleId);
                Assert.Equal(role.RoleName, resultRole.RoleName);
                Assert.Equal(role.Description, resultRole.Description);
                Assert.Equal(role.CreatedBy, resultRole.CreatedBy);
                Assert.Equal(role.CreatedDateTime, resultRole.CreatedDateTime);
                Assert.Equal(role.ModifiedBy, resultRole.ModifiedBy);
                Assert.Equal(role.ModifiedDateTime, resultRole.ModifiedDateTime);
            }
        }
        finally
        {
            // Cleanup
            foreach (var role in roles)
            {
                using var deleteScope = _serviceScopeFactory.CreateScope();
                await using var deleteService = deleteScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await deleteService.RoleRepository.Delete(role.RoleId);
                await deleteService.Commit();
            }
        }

        return;
        
        async Task<Role> AddRole()
        {
            using var addScope = _serviceScopeFactory.CreateScope();
            var addService = addScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var role = RepositoryTestsHelper.CreateRole();
            await addService.RoleRepository.Add(role);
            role.RoleId = await addService.GetLastInsertedId();
            await addService.Commit();
            return role;
        } 
    }
    
    [Fact]
    public async Task GetById_ShouldReturnRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
        
            // Act
            var result = await unitOfWork.RoleRepository.GetById(id);
            
            // Assert
            Assert.Equal(role.RoleId, result.RoleId);
            Assert.Equal(role.RoleName, result.RoleName);
            Assert.Equal(role.Description, result.Description);
            Assert.Equal(role.CreatedBy, result.CreatedBy);
            Assert.Equal(role.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(role.ModifiedBy, result.ModifiedBy);
            Assert.Equal(role.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByParams_ShouldReturnRoles()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
            var parameters = new Dictionary<string, string>
            {
                { "roleId", role.RoleId.ToString() },
                { "roleName", role.RoleName },
                { "description", role.Description },
                { "createdBy", role.CreatedBy },
                { "createdDateTime", role.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
                { "modifiedBy", role.ModifiedBy },
                { "modifiedDateTime", role.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) }
            };
        
            // Act
            var result = await unitOfWork.RoleRepository.GetByParams(parameters);
            
            // Assert
            var resultArray = result as Role[] ?? result.ToArray();
            Assert.NotEmpty(resultArray);
            foreach (var resultRole in resultArray)
            {
                Assert.Equal(role.RoleId, resultRole.RoleId);
                Assert.Equal(role.RoleName, resultRole.RoleName);
                Assert.Equal(role.Description, resultRole.Description);
                Assert.Equal(role.CreatedBy, resultRole.CreatedBy);
                Assert.Equal(role.CreatedDateTime, resultRole.CreatedDateTime);
                Assert.Equal(role.ModifiedBy, resultRole.ModifiedBy);
                Assert.Equal(role.ModifiedDateTime, resultRole.ModifiedDateTime);
            }
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldUpdateRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        scope.ServiceProvider.GetRequiredService<MapperlyMapper>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
            var updatedRole = await unitOfWork.RoleRepository.GetById(id);
            updatedRole.UpdateProperties(RepositoryTestsHelper.CreateRole());
            updatedRole.CreatedBy = RepositoryTestsHelper.RandomString(12);
            updatedRole.CreatedDateTime = DateTime.Now.TrimMiliseconds();
        
            // Act
            await unitOfWork.RoleRepository.Update(updatedRole, RepositoryTestsHelper.OnBeforeUpdate);
            
            // Assert
            var result = await unitOfWork.RoleRepository.GetById(id);
            Assert.Equal(updatedRole.RoleId, result.RoleId);
            Assert.Equal(updatedRole.RoleName, result.RoleName);
            Assert.Equal(updatedRole.Description, result.Description);
            Assert.Equal(updatedRole.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedRole.ModifiedDateTime, result.ModifiedDateTime);
            Assert.NotEqual(updatedRole.CreatedBy, result.CreatedBy);
            Assert.NotEqual(updatedRole.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateRoleIfOriginalValuesAreEmpty()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        scope.ServiceProvider.GetRequiredService<MapperlyMapper>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
            var updatedRole = await unitOfWork.RoleRepository.GetById(id);
        
            // Act
            await unitOfWork.RoleRepository.Update(updatedRole);
            
            // Assert
            var result = await unitOfWork.RoleRepository.GetById(id);
            Assert.Equal(updatedRole.RoleId, result.RoleId);
            Assert.Equal(updatedRole.RoleName, result.RoleName);
            Assert.Equal(updatedRole.Description, result.Description);
            Assert.Equal(updatedRole.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedRole.ModifiedDateTime, result.ModifiedDateTime);
            Assert.Equal(updatedRole.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedRole.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateRoleWhenThereAreNoChanges()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        scope.ServiceProvider.GetRequiredService<MapperlyMapper>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            role.RoleId = id;
            var updatedRole = await unitOfWork.RoleRepository.GetById(id);
            updatedRole.ModifiedBy = RepositoryTestsHelper.RandomString(12);
            updatedRole.ModifiedBy = role.ModifiedBy;
        
            // Act
            await unitOfWork.RoleRepository.Update(updatedRole);
            
            // Assert
            var result = await unitOfWork.RoleRepository.GetById(id);
            Assert.Equal(updatedRole.RoleId, result.RoleId);
            Assert.Equal(updatedRole.RoleName, result.RoleName);
            Assert.Equal(updatedRole.Description, result.Description);
            Assert.Equal(updatedRole.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedRole.ModifiedDateTime, result.ModifiedDateTime);
            Assert.Equal(updatedRole.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedRole.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldThrowExceptionWhenRoleNotFound()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var updatedRole = RepositoryTestsHelper.CreateRole();
            updatedRole.AcceptChanges();
        
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.RoleRepository.Update(updatedRole, RepositoryTestsHelper.OnBeforeUpdate));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task AddUserToRole_ShouldAddUserToRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var roleId = await unitOfWork.GetLastInsertedId();
            role.RoleId = roleId;
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            var userId = await unitOfWork.GetLastInsertedId();
            user.UserId = userId;
        
            // Act
            await unitOfWork.RoleRepository.AddUser(roleId, userId);
            
            // Assert
            var result = await unitOfWork.RoleRepository.GetByIdWithUsers(roleId);
            Assert.NotNull(result.Users.FirstOrDefault(u => u.UserId == userId));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task DeleteUserFromRole_ShouldDeleteUserFromRole()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var roleId = await unitOfWork.GetLastInsertedId();
            role.RoleId = roleId;
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            var userId = await unitOfWork.GetLastInsertedId();
            user.UserId = userId;
            await unitOfWork.RoleRepository.AddUser(roleId, userId);
        
            // Act
            await unitOfWork.RoleRepository.DeleteUser(roleId, userId);
            
            // Assert
            var result = await unitOfWork.RoleRepository.GetByIdWithUsers(roleId);
            Assert.Null(result.Users.FirstOrDefault(u => u.UserId == userId));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAllWithUsers_ShouldReturnAllRolesWithUsers()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            var roleId = await unitOfWork.GetLastInsertedId();
            role.RoleId = roleId;
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            var userId = await unitOfWork.GetLastInsertedId();
            user.UserId = userId;
            await unitOfWork.RoleRepository.AddUser(roleId, userId);
        
            // Act
            var result = await unitOfWork.RoleRepository.GetAllWithUsers();
            
            // Assert
            var resultArray = result as Role[] ?? result.ToArray();
            var resultRole = resultArray.FirstOrDefault(r => r.RoleId == roleId);
            Assert.NotNull(resultRole);
            Assert.NotNull(resultRole.Users.FirstOrDefault(u => u.UserId == userId));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
}