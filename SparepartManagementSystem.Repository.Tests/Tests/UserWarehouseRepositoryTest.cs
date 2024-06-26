using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IUserWarehouseRepository))]
public class UserWarehouseRepositoryTest : IAsyncLifetime
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly User _user = RepositoryTestsHelper.CreateUser();

    public UserWarehouseRepositoryTest()
    {
        var serviceCollectionHelper = new ServiceCollectionHelper();
        _serviceScopeFactory = serviceCollectionHelper.GetRequiredService<IServiceScopeFactory>();
    }
    
    public async Task InitializeAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await unitOfWork.UserRepository.Add(_user);
        _user.UserId = await unitOfWork.GetLastInsertedId();
        await unitOfWork.Commit();
    }

    public async Task DisposeAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await unitOfWork.UserRepository.Delete(_user.UserId);
        await unitOfWork.Commit();
    }
    
    [Fact]
    public async Task Add_ShouldAddUserWarehouse()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);

            // Act
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse, RepositoryTestsHelper.OnBeforeAdd);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();

            // Assert
            var result = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);
            Assert.NotNull(result);
            Assert.Equal(userWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(userWarehouse.UserId, result.UserId);
            Assert.Equal(userWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(userWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(userWarehouse.Name, result.Name);
            Assert.Equal(userWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(userWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(userWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(userWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(userWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteUserWarehouse()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();

            // Act
            await unitOfWork.UserWarehouseRepository.Delete(userWarehouse.UserWarehouseId);

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllUserWarehouses()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            UserWarehouse[] userWarehouses = 
            [
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId),
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId),
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId)
            ];

            foreach (var userWarehouse in userWarehouses)
            {
                await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            }

            // Act
            var result = await unitOfWork.UserWarehouseRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            var collection = result as UserWarehouse[] ?? result.ToArray();
            Assert.NotEmpty(collection);
            Assert.Equal(userWarehouses.Length, collection.Length);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
        
    }
    
    [Fact]
    public async Task GetById_ShouldReturnUserWarehouse()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();

            // Act
            var result = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(userWarehouse.UserId, result.UserId);
            Assert.Equal(userWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(userWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(userWarehouse.Name, result.Name);
            Assert.Equal(userWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(userWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(userWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(userWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(userWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByParams_ShouldReturnUserWarehouses()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();
            
            var parameters = new Dictionary<string, string>
            {
                ["userWarehouseId"] = userWarehouse.UserWarehouseId.ToString(),
                ["userId"] = userWarehouse.UserId.ToString(),
                ["inventLocationId"] = userWarehouse.InventLocationId,
                ["inventSiteId"] = userWarehouse.InventSiteId,
                ["name"] = userWarehouse.Name,
                ["isDefault"] = userWarehouse.IsDefault.ToString(),
                ["createdBy"] = userWarehouse.CreatedBy,
                ["createdDateTime"] = userWarehouse.CreatedDateTime.ToString(CultureInfo.InvariantCulture),
                ["modifiedBy"] = userWarehouse.ModifiedBy,
                ["modifiedDateTime"] = userWarehouse.ModifiedDateTime.ToString(CultureInfo.InvariantCulture)
            };
            
            // Act
            var result = await unitOfWork.UserWarehouseRepository.GetByParams(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldUpdateUserWarehouse()
    {
        // Setup
        using var addNewUserScope = _serviceScopeFactory.CreateScope();
        await using var addNewUserUnitOfWork = addNewUserScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var newUser = RepositoryTestsHelper.CreateUser();
        await addNewUserUnitOfWork.UserRepository.Add(newUser);
        newUser.UserId = await addNewUserUnitOfWork.GetLastInsertedId();
        await addNewUserUnitOfWork.Commit();
        
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();

            var updatedUserWarehouse = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);
            updatedUserWarehouse.UpdateProperties(RepositoryTestsHelper.CreateUserWarehouse(newUser.UserId));
            updatedUserWarehouse.IsDefault = !userWarehouse.IsDefault;
            
            // Act
            await unitOfWork.UserWarehouseRepository.Update(updatedUserWarehouse, RepositoryTestsHelper.OnBeforeUpdate);
            
            // Assert
            var result = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);
            Assert.NotNull(result);
            Assert.Equal(updatedUserWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(updatedUserWarehouse.UserId, result.UserId);
            Assert.Equal(updatedUserWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(updatedUserWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(updatedUserWarehouse.Name, result.Name);
            Assert.Equal(updatedUserWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(updatedUserWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedUserWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedUserWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedUserWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
            
            using var deleteNewUserScope = _serviceScopeFactory.CreateScope();
            await using var deleteNewUserUnitOfWork = deleteNewUserScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await deleteNewUserUnitOfWork.UserRepository.Delete(newUser.UserId);
            await deleteNewUserUnitOfWork.Commit();
        }
    }

    [Fact]
    public async Task Update_ShouldNotUpdateUserWhenThereAreNoChanges()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();
            var updatedUserWarehouse = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);
            updatedUserWarehouse.ModifiedBy = RepositoryTestsHelper.RandomString(12);
            updatedUserWarehouse.ModifiedBy = userWarehouse.ModifiedBy; 
            
            // Act
            await unitOfWork.UserWarehouseRepository.Update(updatedUserWarehouse);
            
            // Assert
            var result = await unitOfWork.UserWarehouseRepository.GetById(updatedUserWarehouse.UserWarehouseId);
            Assert.NotNull(result);
            Assert.Equal(updatedUserWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(updatedUserWarehouse.UserId, result.UserId);
            Assert.Equal(updatedUserWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(updatedUserWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(updatedUserWarehouse.Name, result.Name);
            Assert.Equal(updatedUserWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(updatedUserWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedUserWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedUserWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedUserWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateUserWhenOriginalValuesAreEmpty()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            userWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();
            var updatedUserWarehouse = await unitOfWork.UserWarehouseRepository.GetById(userWarehouse.UserWarehouseId);
            
            // Act
            await unitOfWork.UserWarehouseRepository.Update(updatedUserWarehouse);
            
            // Assert
            var result = await unitOfWork.UserWarehouseRepository.GetById(updatedUserWarehouse.UserWarehouseId);
            Assert.NotNull(result);
            Assert.Equal(updatedUserWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(updatedUserWarehouse.UserId, result.UserId);
            Assert.Equal(updatedUserWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(updatedUserWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(updatedUserWarehouse.Name, result.Name);
            Assert.Equal(updatedUserWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(updatedUserWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedUserWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedUserWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedUserWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldThrowExceptionWhenUserWarehouseNotFound()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var userWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            userWarehouse.AcceptChanges();
            userWarehouse.ModifiedBy = RepositoryTestsHelper.RandomString(12);
            
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.UserWarehouseRepository.Update(userWarehouse));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetByUserId_ShouldReturnUserWarehouses()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            UserWarehouse[] userWarehouses = 
            [
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId),
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId),
                RepositoryTestsHelper.CreateUserWarehouse(_user.UserId)
            ];

            foreach (var userWarehouse in userWarehouses)
            {
                await unitOfWork.UserWarehouseRepository.Add(userWarehouse);
            }

            // Act
            var result = await unitOfWork.UserWarehouseRepository.GetByUserId(_user.UserId);

            // Assert
            Assert.NotNull(result);
            var userWarehousesResult = result as UserWarehouse[] ?? result.ToArray();
            Assert.NotEmpty(userWarehousesResult);
            Assert.Equal(userWarehouses.Length, userWarehousesResult.Length);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetDefaultByUserId_ShouldReturnDefaultUserWarehouse()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var defaultUserWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            defaultUserWarehouse.IsDefault = true;
            await unitOfWork.UserWarehouseRepository.Add(defaultUserWarehouse);
            defaultUserWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();
            
            var notDefaultUserWarehouse = RepositoryTestsHelper.CreateUserWarehouse(_user.UserId);
            notDefaultUserWarehouse.IsDefault = false;
            await unitOfWork.UserWarehouseRepository.Add(notDefaultUserWarehouse);
            notDefaultUserWarehouse.UserWarehouseId = await unitOfWork.GetLastInsertedId();
            
            // Act
            var result = await unitOfWork.UserWarehouseRepository.GetDefaultByUserId(_user.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(defaultUserWarehouse.UserWarehouseId, result.UserWarehouseId);
            Assert.Equal(defaultUserWarehouse.UserId, result.UserId);
            Assert.Equal(defaultUserWarehouse.InventLocationId, result.InventLocationId);
            Assert.Equal(defaultUserWarehouse.InventSiteId, result.InventSiteId);
            Assert.Equal(defaultUserWarehouse.Name, result.Name);
            Assert.Equal(defaultUserWarehouse.IsDefault, result.IsDefault);
            Assert.Equal(defaultUserWarehouse.CreatedBy, result.CreatedBy);
            Assert.Equal(defaultUserWarehouse.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(defaultUserWarehouse.ModifiedBy, result.ModifiedBy);
            Assert.Equal(defaultUserWarehouse.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
}