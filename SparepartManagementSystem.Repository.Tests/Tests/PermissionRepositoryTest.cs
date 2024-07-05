using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

public class PermissionRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Role _role = RepositoryTestsHelper.CreateRole();

    public PermissionRepositoryTest()
    {
        _serviceScopeFactory = _serviceCollectionHelper.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task InitializeAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            await unitOfWork.RoleRepository.Add(_role);
            _role.RoleId = await unitOfWork.GetLastInsertedId();
            await unitOfWork.Commit();
        }
        catch (Exception)
        {
            await unitOfWork.Rollback();
        }
    }

    public async Task DisposeAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            await unitOfWork.RoleRepository.Delete(_role.RoleId);
            await unitOfWork.Commit();
        }
        catch (Exception)
        {
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Add_ShouldAddPermission()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
        
            // Act
            await unitOfWork.PermissionRepository.Add(permission);
        
            // Assert
            var id = await unitOfWork.GetLastInsertedId();
            var result = await unitOfWork.PermissionRepository.GetById(id);
            result.PermissionId = id;
            Assert.True(result.PermissionId > 0);
            Assert.Equal(permission.PermissionName, result.PermissionName);
            Assert.Equal(permission.RoleId, result.RoleId);
            Assert.Equal(permission.Module, result.Module);
            Assert.Equal(permission.Type, result.Type);
            Assert.Equal(permission.CreatedBy, result.CreatedBy);
            Assert.Equal(permission.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(permission.ModifiedBy, result.ModifiedBy);
            Assert.Equal(permission.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Delete_ShouldDeletePermission()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await unitOfWork.PermissionRepository.Add(permission);
            var id = await unitOfWork.GetLastInsertedId();
        
            // Act
            await unitOfWork.PermissionRepository.Delete(id);
        
            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => unitOfWork.PermissionRepository.GetById(id));
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllPermissions()
    {
        // Setup
        Permission[] permissions = [];
        
        try
        {
            // Arrange
            var tasks = new List<Task<Permission>>
            {
                Task.Run(Permission),
                Task.Run(Permission),
                Task.Run(Permission),
            };
            permissions = await Task.WhenAll(tasks);
        
            // Act
            using var scopeGetAll = _serviceScopeFactory.CreateScope();
            await using var getAllService = scopeGetAll.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var result = await getAllService.PermissionRepository.GetAll();
        
            // Assert
            var resultArray = result as Permission[] ?? result.ToArray();
            Assert.Equal(permissions.Length, resultArray.Length);
            foreach (var permission in permissions)
            {
                var resultPermission = resultArray.Single(p => p.PermissionId == permission.PermissionId);
                Assert.Equal(permission.PermissionName, resultPermission.PermissionName);
                Assert.Equal(permission.RoleId, resultPermission.RoleId);
                Assert.Equal(permission.Module, resultPermission.Module);
                Assert.Equal(permission.Type, resultPermission.Type);
                Assert.Equal(permission.CreatedBy, resultPermission.CreatedBy);
                Assert.Equal(permission.CreatedDateTime, resultPermission.CreatedDateTime);
                Assert.Equal(permission.ModifiedBy, resultPermission.ModifiedBy);
                Assert.Equal(permission.ModifiedDateTime, resultPermission.ModifiedDateTime);
            }
        }
        finally
        {
            // Teardown
            foreach (var permission in permissions)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                await using var deleteService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await deleteService.PermissionRepository.Delete(permission.PermissionId);
                await deleteService.Commit();
            }
        }

        return;

        async Task<Permission> Permission()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await using var addService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await addService.PermissionRepository.Add(permission);
            permission.PermissionId = await addService.GetLastInsertedId();
            await addService.Commit();
            return permission;
        }
    }
    
    [Fact]
    public async Task GetByRoleId_ShouldReturnAllPermissionsByRoleId()
    {
        // Setup
        Permission[] permissions = [];
        
        try
        {
            // Arrange
            var tasks = new List<Task<Permission>>
            {
                Task.Run(Permission),
                Task.Run(Permission),
                Task.Run(Permission),
            };
            permissions = await Task.WhenAll(tasks);
        
            // Act
            using var getByRoleIdScope = _serviceScopeFactory.CreateScope();
            await using var getByRoleIdService = getByRoleIdScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var result = await getByRoleIdService.PermissionRepository.GetByRoleId(_role.RoleId);
        
            // Assert
            var resultArray = result as Permission[] ?? result.ToArray();
            Assert.Equal(permissions.Length, resultArray.Length);
            foreach (var permission in permissions)
            {
                var resultPermission = resultArray.Single(p => p.PermissionId == permission.PermissionId);
                Assert.Equal(permission.PermissionName, resultPermission.PermissionName);
                Assert.Equal(permission.RoleId, resultPermission.RoleId);
                Assert.Equal(permission.Module, resultPermission.Module);
                Assert.Equal(permission.Type, resultPermission.Type);
                Assert.Equal(permission.CreatedBy, resultPermission.CreatedBy);
                Assert.Equal(permission.CreatedDateTime, resultPermission.CreatedDateTime);
                Assert.Equal(permission.ModifiedBy, resultPermission.ModifiedBy);
                Assert.Equal(permission.ModifiedDateTime, resultPermission.ModifiedDateTime);
            }
        }
        finally
        {
            // Teardown
            foreach (var permission in permissions)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                await using var deleteService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await deleteService.PermissionRepository.Delete(permission.PermissionId);
                await deleteService.Commit();
            }
        }

        return;

        async Task<Permission> Permission()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await using var addService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await addService.PermissionRepository.Add(permission);
            permission.PermissionId = await addService.GetLastInsertedId();
            await addService.Commit();
            return permission;
        }
    }
    
    [Fact]
    public async Task GetById_ShouldReturnPermissionById()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            permission.RoleId = _role.RoleId;
            await unitOfWork.PermissionRepository.Add(permission);
            var id = await unitOfWork.GetLastInsertedId();
        
            // Act
            var result = await unitOfWork.PermissionRepository.GetById(id);
        
            // Assert
            Assert.Equal(permission.PermissionName, result.PermissionName);
            Assert.Equal(permission.RoleId, result.RoleId);
            Assert.Equal(permission.Module, result.Module);
            Assert.Equal(permission.Type, result.Type);
            Assert.Equal(permission.CreatedBy, result.CreatedBy);
            Assert.Equal(permission.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(permission.ModifiedBy, result.ModifiedBy);
            Assert.Equal(permission.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldUpdatePermission()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await unitOfWork.PermissionRepository.Add(permission, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            var newPermission = await unitOfWork.PermissionRepository.GetById(id);
            var newRole = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(newRole);
            newRole.RoleId = await unitOfWork.GetLastInsertedId();
            newPermission.UpdateProperties(RepositoryTestsHelper.CreatePermission(newRole.RoleId));
            newPermission.CreatedBy = RepositoryTestsHelper.RandomString(12);
            newPermission.CreatedDateTime = RepositoryTestsHelper.RandomDateTime();

            // Act
            await unitOfWork.PermissionRepository.Update(newPermission, RepositoryTestsHelper.OnBeforeUpdate);
        
            // Assert
            var result = await unitOfWork.PermissionRepository.GetById(id);
            Assert.Equal(newPermission.PermissionId, result.PermissionId);
            Assert.Equal(newPermission.PermissionName, result.PermissionName);
            Assert.Equal(newPermission.RoleId, result.RoleId);
            Assert.Equal(newPermission.Module, result.Module);
            Assert.Equal(newPermission.Type, result.Type);
            Assert.Equal(newPermission.ModifiedBy, result.ModifiedBy);
            Assert.Equal(newPermission.ModifiedDateTime, result.ModifiedDateTime);
            Assert.NotEqual(newPermission.CreatedBy, result.CreatedBy);
            Assert.NotEqual(newPermission.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotModifyEntityIfOriginalValuesAreEmpty()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await unitOfWork.PermissionRepository.Add(permission, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            var newPermission = await unitOfWork.PermissionRepository.GetById(id);

            // Act
            await unitOfWork.PermissionRepository.Update(newPermission);
        
            // Assert
            var result = await unitOfWork.PermissionRepository.GetById(id);
            Assert.Equal(newPermission.PermissionId, result.PermissionId);
            Assert.Equal(newPermission.PermissionName, result.PermissionName);
            Assert.Equal(newPermission.RoleId, result.RoleId);
            Assert.Equal(newPermission.Module, result.Module);
            Assert.Equal(newPermission.Type, result.Type);
            Assert.Equal(newPermission.ModifiedBy, result.ModifiedBy);
            Assert.Equal(newPermission.ModifiedDateTime, result.ModifiedDateTime);
            Assert.Equal(newPermission.CreatedBy, result.CreatedBy);
            Assert.Equal(newPermission.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotModifyEntityIfThereAreNoChanges()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await unitOfWork.PermissionRepository.Add(permission, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            var newPermission = await unitOfWork.PermissionRepository.GetById(id);
            newPermission.ModifiedBy = RepositoryTestsHelper.RandomString(12);
            newPermission.ModifiedBy = permission.ModifiedBy;

            // Act
            await unitOfWork.PermissionRepository.Update(newPermission);
        
            // Assert
            var result = await unitOfWork.PermissionRepository.GetById(id);
            Assert.Equal(newPermission.PermissionId, result.PermissionId);
            Assert.Equal(newPermission.PermissionName, result.PermissionName);
            Assert.Equal(newPermission.RoleId, result.RoleId);
            Assert.Equal(newPermission.Module, result.Module);
            Assert.Equal(newPermission.Type, result.Type);
            Assert.Equal(newPermission.ModifiedBy, result.ModifiedBy);
            Assert.Equal(newPermission.ModifiedDateTime, result.ModifiedDateTime);
            Assert.Equal(newPermission.CreatedBy, result.CreatedBy);
            Assert.Equal(newPermission.CreatedDateTime, result.CreatedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldThrowExceptionWhenPermissionNotFound()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        // Arrange
        var newPermission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
        newPermission.AcceptChanges();
        newPermission.PermissionName = RepositoryTestsHelper.RandomString(12);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => unitOfWork.PermissionRepository.Update(newPermission));
    }


    [Fact]
    public async Task GetByParams_ShouldReturnPermissionsByParams()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            var permission = RepositoryTestsHelper.CreatePermission(_role.RoleId);
            await unitOfWork.PermissionRepository.Add(permission, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            permission.PermissionId = id;
        
            // Act
            var parameters = new Dictionary<string, string>
            {
                { "permissionId", permission.PermissionId.ToString() },
                { "permissionName", permission.PermissionName },
                { "roleId", permission.RoleId.ToString() },
                { "module", permission.Module },
                { "type", permission.Type },
                { "createdBy", permission.CreatedBy },
                { "createdDateTime", permission.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
                { "modifiedBy", permission.ModifiedBy },
                { "modifiedDateTime", permission.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) },
            };
            var result = await unitOfWork.PermissionRepository.GetByParams(parameters);
        
            // Assert
            var resultArray = result as Permission[] ?? result.ToArray();
            Assert.Single(resultArray);
            var resultPermission = resultArray.Single();
            Assert.Equal(permission.PermissionId, resultPermission.PermissionId);
            Assert.Equal(permission.PermissionName, resultPermission.PermissionName);
            Assert.Equal(permission.RoleId, resultPermission.RoleId);
            Assert.Equal(permission.Module, resultPermission.Module);
            Assert.Equal(permission.Type, resultPermission.Type);
            Assert.Equal(permission.CreatedBy, resultPermission.CreatedBy);
            Assert.Equal(permission.CreatedDateTime, resultPermission.CreatedDateTime);
            Assert.Equal(permission.ModifiedBy, resultPermission.ModifiedBy);
            Assert.Equal(permission.ModifiedDateTime, resultPermission.ModifiedDateTime);
        }
        finally
        {
            // Teardown
            await unitOfWork.Rollback();
        }
    }
}