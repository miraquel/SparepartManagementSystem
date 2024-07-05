using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IUserRepository))]
public class UserRepositoryTest
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserRepositoryTest()
    {
        _serviceScopeFactory = _serviceCollectionHelper.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Add_ShouldAddUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();

            // Act
            await unitOfWork.UserRepository.Add(user);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;

            // Assert
            var result = await unitOfWork.UserRepository.GetById(id);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;

            // Act
            await unitOfWork.UserRepository.Delete(id);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.UserRepository.GetById(id));
            
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldUpdateUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;
            var updatedUser = await unitOfWork.UserRepository.GetById(id);
            updatedUser.UpdateProperties(RepositoryTestsHelper.CreateUser());
            updatedUser.IsAdministrator = !user.IsAdministrator;
            updatedUser.IsEnabled = !user.IsEnabled;

            // Act
            await unitOfWork.UserRepository.Update(updatedUser, RepositoryTestsHelper.OnBeforeUpdate);

            // Assert
            var result = await unitOfWork.UserRepository.GetById(id);
            Assert.Equal(updatedUser.UserId, result.UserId);
            Assert.Equal(updatedUser.Username, result.Username);
            Assert.Equal(updatedUser.Email, result.Email);
            Assert.Equal(updatedUser.FirstName, result.FirstName);
            Assert.Equal(updatedUser.LastName, result.LastName);
            Assert.Equal(updatedUser.IsAdministrator, result.IsAdministrator);
            Assert.Equal(updatedUser.IsEnabled, result.IsEnabled);
            Assert.Equal(updatedUser.LastLogin, result.LastLogin);
            Assert.Equal(updatedUser.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedUser.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedUser.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedUser.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateIfThereAreNoChanges()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;
            var updatedUser = await unitOfWork.UserRepository.GetById(id);

            // Act
            await unitOfWork.UserRepository.Update(updatedUser);

            // Assert
            var result = await unitOfWork.UserRepository.GetById(id);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateIfOriginalValuesAreEmpty()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user, RepositoryTestsHelper.OnBeforeAdd);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;
            var updatedUser = await unitOfWork.UserRepository.GetById(id);
            updatedUser.ModifiedBy = RepositoryTestsHelper.RandomString(12);
            updatedUser.ModifiedBy = user.ModifiedBy;

            // Act
            await unitOfWork.UserRepository.Update(updatedUser);

            // Assert
            var result = await unitOfWork.UserRepository.GetById(id);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldThrowExceptionIfUserNotFound()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var user = RepositoryTestsHelper.CreateUser();
        user.AcceptChanges();
        user.FirstName = RepositoryTestsHelper.RandomString(12);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.UserRepository.Update(user));
    }
    
    [Fact]
    public async Task GetById_ShouldReturnUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            var id = await unitOfWork.GetLastInsertedId();
            user.UserId = id;

            // Act
            var result = await unitOfWork.UserRepository.GetById(id);

            // Assert
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByIdWithRoles_ShouldReturnUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user, RepositoryTestsHelper.OnBeforeAdd);
            user.UserId = await unitOfWork.GetLastInsertedId();
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role, RepositoryTestsHelper.OnBeforeAdd);
            role.RoleId = await unitOfWork.GetLastInsertedId();
            await unitOfWork.RoleRepository.AddUser(role.RoleId, user.UserId);

            // Act
            var result = await unitOfWork.UserRepository.GetByIdWithRoles(user.UserId);

            // Assert
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
            Assert.NotEmpty(result.Roles);
            var resultRole = Assert.Single(result.Roles);
            Assert.Equal(role.RoleId, resultRole.RoleId);
            Assert.Equal(role.RoleName, resultRole.RoleName);
            Assert.Equal(role.Description, resultRole.Description);
            Assert.Equal(role.CreatedBy, resultRole.CreatedBy);
            Assert.Equal(role.CreatedDateTime, resultRole.CreatedDateTime);
            Assert.Equal(role.ModifiedBy, resultRole.ModifiedBy);
            Assert.Equal(role.ModifiedDateTime, resultRole.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByUsername_ShouldReturnUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            user.UserId = await unitOfWork.GetLastInsertedId();

            // Act
            var result = await unitOfWork.UserRepository.GetByUsername(user.Username);

            // Assert
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByUsernameWithRoles_ShouldReturnUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            user.UserId = await unitOfWork.GetLastInsertedId();
            var role = RepositoryTestsHelper.CreateRole();
            await unitOfWork.RoleRepository.Add(role);
            role.RoleId = await unitOfWork.GetLastInsertedId();
            await unitOfWork.RoleRepository.AddUser(role.RoleId, user.UserId);

            // Act
            var result = await unitOfWork.UserRepository.GetByUsernameWithRoles(user.Username);

            // Assert
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
            Assert.NotEmpty(result.Roles);
            var resultRole = Assert.Single(result.Roles);
            Assert.Equal(role.RoleId, resultRole.RoleId);
            Assert.Equal(role.RoleName, resultRole.RoleName);
            Assert.Equal(role.Description, resultRole.Description);
            Assert.Equal(role.CreatedBy, resultRole.CreatedBy);
            Assert.Equal(role.CreatedDateTime, resultRole.CreatedDateTime);
            Assert.Equal(role.ModifiedBy, resultRole.ModifiedBy);
            Assert.Equal(role.ModifiedDateTime, resultRole.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByIdWithUserWarehouse_ShouldReturnUser()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var user = RepositoryTestsHelper.CreateUser();
            await unitOfWork.UserRepository.Add(user);
            user.UserId = await unitOfWork.GetLastInsertedId();
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();

            // Act
            var result = await unitOfWork.UserRepository.GetByIdWithUserWarehouse(user.UserId);

            // Assert
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.IsAdministrator, result.IsAdministrator);
            Assert.Equal(user.IsEnabled, result.IsEnabled);
            Assert.Equal(user.LastLogin, result.LastLogin);
            Assert.Equal(user.CreatedBy, result.CreatedBy);
            Assert.Equal(user.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(user.ModifiedBy, result.ModifiedBy);
            Assert.Equal(user.ModifiedDateTime, result.ModifiedDateTime);
            Assert.NotEmpty(result.UserWarehouses);
            var resultUserWarehouse = Assert.Single(result.UserWarehouses);
            Assert.Equal(userWarehouse.UserWarehouseId, resultUserWarehouse.UserWarehouseId);
            Assert.Equal(userWarehouse.UserId, resultUserWarehouse.UserId);
            Assert.Equal(userWarehouse.InventLocationId, resultUserWarehouse.InventLocationId);
            Assert.Equal(userWarehouse.InventSiteId, resultUserWarehouse.InventSiteId);
            Assert.Equal(userWarehouse.IsDefault, resultUserWarehouse.IsDefault);
            Assert.Equal(userWarehouse.CreatedBy, resultUserWarehouse.CreatedBy);
            Assert.Equal(userWarehouse.CreatedDateTime, resultUserWarehouse.CreatedDateTime);
            Assert.Equal(userWarehouse.ModifiedBy, resultUserWarehouse.ModifiedBy);
            Assert.Equal(userWarehouse.ModifiedDateTime, resultUserWarehouse.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnUsers()
    {
        // Setup
        User[] users = [];
        
        try
        {
            // Arrange
            var tasks = new List<Task<User>>
            {
                Task.Run(AddUser),
                Task.Run(AddUser),
                Task.Run(AddUser)
            };
            
            users = await Task.WhenAll(tasks);

            // Act
            using var getAllScope = _serviceScopeFactory.CreateScope();
            await using var getAllUnitOfWork = getAllScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var result = await getAllUnitOfWork.UserRepository.GetAll();

            // Assert
            var resultUsers = result as User[] ?? result.ToArray();
            foreach (var user in users)
            {
                var resultUser = resultUsers.FirstOrDefault(x => x.UserId == user.UserId);
                Assert.NotNull(resultUser);
                Assert.Equal(user.UserId, resultUser.UserId);
                Assert.Equal(user.Username, resultUser.Username);
                Assert.Equal(user.Email, resultUser.Email);
                Assert.Equal(user.FirstName, resultUser.FirstName);
                Assert.Equal(user.LastName, resultUser.LastName);
                Assert.Equal(user.IsAdministrator, resultUser.IsAdministrator);
                Assert.Equal(user.IsEnabled, resultUser.IsEnabled);
                Assert.Equal(user.LastLogin, resultUser.LastLogin);
                Assert.Equal(user.CreatedBy, resultUser.CreatedBy);
                Assert.Equal(user.CreatedDateTime, resultUser.CreatedDateTime);
                Assert.Equal(user.ModifiedBy, resultUser.ModifiedBy);
                Assert.Equal(user.ModifiedDateTime, resultUser.ModifiedDateTime);
            }
            
            async Task<User> AddUser()
            {
                using var addUserScope = _serviceScopeFactory.CreateScope();
                await using var addUserUnitOfWork = addUserScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var user = RepositoryTestsHelper.CreateUser();
                await addUserUnitOfWork.UserRepository.Add(user);
                user.UserId = await addUserUnitOfWork.GetLastInsertedId();
                await addUserUnitOfWork.Commit();
                return user;
            }
        }
        finally
        {
            // Cleanup
            foreach (var user in users)
            {
                using var deleteUserScope = _serviceScopeFactory.CreateScope();
                await using var deleteUserUnitOfWork = deleteUserScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await deleteUserUnitOfWork.UserRepository.Delete(user.UserId);
                await deleteUserUnitOfWork.Commit();
            }
        }
    }
    
    [Fact]
    public async Task GetByParams_ShouldReturnUsers()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        // Arrange
        var user = RepositoryTestsHelper.CreateUser();
        await unitOfWork.UserRepository.Add(user, RepositoryTestsHelper.OnBeforeAdd);
        user.UserId = await unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            { "userId", user.UserId.ToString() },
            { "username", user.Username },
            { "email", user.Email },
            { "firstName", user.FirstName },
            { "lastName", user.LastName },
            { "isAdministrator", user.IsAdministrator.ToString() },
            { "isEnabled", user.IsEnabled.ToString() },
            { "lastLogin", user.LastLogin.ToString(CultureInfo.InvariantCulture) },
            { "createdBy", user.CreatedBy },
            { "createdDateTime", user.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
            { "modifiedBy", user.ModifiedBy },
            { "modifiedDateTime", user.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) }
        };
        
        // Act
        var result = await unitOfWork.UserRepository.GetByParams(parameters);
        
        // Assert
        var resultUser = Assert.Single(result);
        Assert.Equal(user.UserId, resultUser.UserId);
        Assert.Equal(user.Username, resultUser.Username);
        Assert.Equal(user.Email, resultUser.Email);
        Assert.Equal(user.FirstName, resultUser.FirstName);
        Assert.Equal(user.LastName, resultUser.LastName);
        Assert.Equal(user.IsAdministrator, resultUser.IsAdministrator);
        Assert.Equal(user.IsEnabled, resultUser.IsEnabled);
        Assert.Equal(user.LastLogin, resultUser.LastLogin);
        Assert.Equal(user.CreatedBy, resultUser.CreatedBy);
        Assert.Equal(user.CreatedDateTime, resultUser.CreatedDateTime);
        Assert.Equal(user.ModifiedBy, resultUser.ModifiedBy);
        Assert.Equal(user.ModifiedDateTime, resultUser.ModifiedDateTime);
    }
    
    // GetAllWithRoles
    [Fact]
    public async Task GetAllWithRoles_ShouldReturnUsersWithRoles()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Arrange
            User[] users = [];
            Role[] roles = [];
            
            for (var i = 0; i < 3; i++)
            {
                var user = await AddUser();
                users = users.Append(user).ToArray();
            }

            for (var i = 0; i < 3; i++)
            {
                var role = await AddRole();
                roles = roles.Append(role).ToArray();
            }

            var addRoleTasks = from user in users from role in roles select AddUserRole(role.RoleId, user.UserId);
            foreach (var addRoleTask in addRoleTasks)
            {
                await addRoleTask;
            }

            // Act
            var result = await unitOfWork.UserRepository.GetAllWithRoles();
            
            // Assert
            var resultUsers = result as User[] ?? result.ToArray();
            foreach (var user in users)
            {
                var resultUser = resultUsers.FirstOrDefault(x => x.UserId == user.UserId);
                Assert.NotNull(resultUser);
                Assert.Equal(user.UserId, resultUser.UserId);
                Assert.Equal(user.Username, resultUser.Username);
                Assert.Equal(user.Email, resultUser.Email);
                Assert.Equal(user.FirstName, resultUser.FirstName);
                Assert.Equal(user.LastName, resultUser.LastName);
                Assert.Equal(user.IsAdministrator, resultUser.IsAdministrator);
                Assert.Equal(user.IsEnabled, resultUser.IsEnabled);
                Assert.Equal(user.LastLogin, resultUser.LastLogin);
                Assert.Equal(user.CreatedBy, resultUser.CreatedBy);
                Assert.Equal(user.CreatedDateTime, resultUser.CreatedDateTime);
                Assert.Equal(user.ModifiedBy, resultUser.ModifiedBy);
                Assert.Equal(user.ModifiedDateTime, resultUser.ModifiedDateTime);
                Assert.NotEmpty(resultUser.Roles);
                foreach (var role in roles)
                {
                    var resultRole = resultUser.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);
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
            
            async Task<User> AddUser()
            {
                var user = RepositoryTestsHelper.CreateUser();
                await unitOfWork.UserRepository.Add(user);
                user.UserId = await unitOfWork.GetLastInsertedId();
                return user;
            }
            
            async Task<Role> AddRole()
            {
                var role = RepositoryTestsHelper.CreateRole();
                await unitOfWork.RoleRepository.Add(role);
                role.RoleId = await unitOfWork.GetLastInsertedId();
                return role;
            }

            async Task AddUserRole(int roleId, int userId)
            {
                await unitOfWork.RoleRepository.AddUser(roleId, userId);
            }
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
            await unitOfWork.DisposeAsync();
        }
    }
}