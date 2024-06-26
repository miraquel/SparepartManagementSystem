using System.Globalization;
using JetBrains.Annotations;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IRowLevelAccessRepository))]
public class RowLevelAccessRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IUnitOfWork _unitOfWork;
    private readonly User _user = RepositoryTestsHelper.CreateUser();

    public RowLevelAccessRepositoryTest()
    {
        _unitOfWork = _serviceCollectionHelper.GetRequiredService<IUnitOfWork>();
    }
    
    public async Task InitializeAsync()
    {
        await _unitOfWork.UserRepository.Add(_user);
        _user.UserId = await _unitOfWork.GetLastInsertedId();
    }

    public async Task DisposeAsync()
    {
        await _unitOfWork.Rollback();
    }
    
    [Fact]
    public async Task Add_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;

        // Act
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);

        // Assert
        var result = await _unitOfWork.RowLevelAccessRepository.GetById(await _unitOfWork.GetLastInsertedId());
        Assert.True(rowLevelAccess.RowLevelAccessId > 0);
        Assert.Equal(rowLevelAccess.UserId, result.UserId);
        Assert.Equal(rowLevelAccess.AxTable, result.AxTable);
        Assert.Equal(rowLevelAccess.Query, result.Query);
        Assert.Equal(rowLevelAccess.CreatedBy, result.CreatedBy);
        Assert.Equal(rowLevelAccess.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(rowLevelAccess.ModifiedBy, result.ModifiedBy);
        Assert.Equal(rowLevelAccess.ModifiedDateTime, result.ModifiedDateTime);
    }
    
    [Fact]
    public async Task GetById_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);

        // Act
        var result = await _unitOfWork.RowLevelAccessRepository.GetById(await _unitOfWork.GetLastInsertedId());

        // Assert
        Assert.True(rowLevelAccess.RowLevelAccessId > 0);
        Assert.Equal(rowLevelAccess.UserId, result.UserId);
        Assert.Equal(rowLevelAccess.AxTable, result.AxTable);
        Assert.Equal(rowLevelAccess.Query, result.Query);
        Assert.Equal(rowLevelAccess.CreatedBy, result.CreatedBy);
        Assert.Equal(rowLevelAccess.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(rowLevelAccess.ModifiedBy, result.ModifiedBy);
        Assert.Equal(rowLevelAccess.ModifiedDateTime, result.ModifiedDateTime);
    }
    
    [Fact]
    public async Task GetAll_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);

        // Act
        var result = await _unitOfWork.RowLevelAccessRepository.GetAll();

        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task Update_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        var id = await _unitOfWork.GetLastInsertedId();
        var anotherUser = RepositoryTestsHelper.CreateUser();
        await _unitOfWork.UserRepository.Add(anotherUser);
        var anotherUserId = await _unitOfWork.GetLastInsertedId();
        var updatedRowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetById(id);
        var axTables = Enum.GetValues<AxTable>().Where(a => a != updatedRowLevelAccess.AxTable).ToArray();
        updatedRowLevelAccess.UserId = anotherUserId;
        updatedRowLevelAccess.AxTable = axTables[RepositoryTestsHelper.Random.Next(axTables.Length)];
        updatedRowLevelAccess.Query = RepositoryTestsHelper.RandomString(12);
        updatedRowLevelAccess.CreatedBy = RepositoryTestsHelper.RandomString(12);
        updatedRowLevelAccess.CreatedDateTime = DateTime.Now.AddDays(1).TrimMiliseconds();
        updatedRowLevelAccess.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedRowLevelAccess.ModifiedDateTime = DateTime.Now.AddDays(1).TrimMiliseconds();

        // Act
        await _unitOfWork.RowLevelAccessRepository.Update(updatedRowLevelAccess);

        // Assert
        var result = await _unitOfWork.RowLevelAccessRepository.GetById(id);
        Assert.Equal(updatedRowLevelAccess.RowLevelAccessId, result.RowLevelAccessId);
        Assert.Equal(updatedRowLevelAccess.UserId, result.UserId);
        Assert.Equal(updatedRowLevelAccess.AxTable, result.AxTable);
        Assert.Equal(updatedRowLevelAccess.Query, result.Query);
        Assert.Equal(updatedRowLevelAccess.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedRowLevelAccess.ModifiedDateTime, result.ModifiedDateTime);
        Assert.NotEqual(updatedRowLevelAccess.CreatedBy, result.CreatedBy);
        Assert.NotEqual(updatedRowLevelAccess.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateWhenThereAreNoChanges()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess(_user.UserId);
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedRowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetById(id);

        // Act
        await _unitOfWork.RowLevelAccessRepository.Update(updatedRowLevelAccess);

        // Assert
        var result = await _unitOfWork.RowLevelAccessRepository.GetById(id);
        Assert.Equal(updatedRowLevelAccess.RowLevelAccessId, result.RowLevelAccessId);
        Assert.Equal(updatedRowLevelAccess.UserId, result.UserId);
        Assert.Equal(updatedRowLevelAccess.AxTable, result.AxTable);
        Assert.Equal(updatedRowLevelAccess.Query, result.Query);
        Assert.Equal(updatedRowLevelAccess.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedRowLevelAccess.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedRowLevelAccess.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedRowLevelAccess.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateIfOriginalValuesAreEmpty()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess(_user.UserId);
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedRowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetById(id);
        updatedRowLevelAccess.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedRowLevelAccess.ModifiedBy = rowLevelAccess.ModifiedBy;

        // Act
        await _unitOfWork.RowLevelAccessRepository.Update(updatedRowLevelAccess);

        // Assert
        var result = await _unitOfWork.RowLevelAccessRepository.GetById(id);
        Assert.Equal(updatedRowLevelAccess.RowLevelAccessId, result.RowLevelAccessId);
        Assert.Equal(updatedRowLevelAccess.UserId, result.UserId);
        Assert.Equal(updatedRowLevelAccess.AxTable, result.AxTable);
        Assert.Equal(updatedRowLevelAccess.Query, result.Query);
        Assert.Equal(updatedRowLevelAccess.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedRowLevelAccess.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedRowLevelAccess.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedRowLevelAccess.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Delete_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        await _unitOfWork.RowLevelAccessRepository.Delete(id);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RowLevelAccessRepository.GetById(id));
    }
    
    [Fact]
    public async Task Update_ShouldThrowInvalidOperationExceptionWhenEntityDoesNotExist()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.AcceptChanges();
        rowLevelAccess.UserId = _user.UserId;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RowLevelAccessRepository.Update(rowLevelAccess));
    }
    
    [Fact]
    public async Task GetById_ShouldThrowInvalidOperationExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RowLevelAccessRepository.GetById(0));
    }
    
    [Fact]
    public async Task Delete_ShouldThrowInvalidOperationExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RowLevelAccessRepository.Delete(0));
    }
    
    [Fact]
    public async Task Add_ShouldThrowInvalidOperationExceptionWhenUserIdDoesNotExist()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = 0;

        // Act & Assert
        await Assert.ThrowsAsync<MySqlException>(() => _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess));
    }
    
    [Fact]
    public async Task GetByParams_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        rowLevelAccess.RowLevelAccessId = await _unitOfWork.GetLastInsertedId();
        
        var parameters = new Dictionary<string, string>
        {
            { "rowLevelAccessId", rowLevelAccess.RowLevelAccessId.ToString() },
            { "userId", rowLevelAccess.UserId.ToString() },
            { "axTable", ((int)rowLevelAccess.AxTable).ToString() },
            { "query", rowLevelAccess.Query },
            { "createdBy", rowLevelAccess.CreatedBy },
            { "createdDateTime", rowLevelAccess.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
            { "modifiedBy", rowLevelAccess.ModifiedBy },
            { "modifiedDateTime", rowLevelAccess.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) }
        };

        // Act
        var result = await _unitOfWork.RowLevelAccessRepository.GetByParams(parameters);

        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetByUserId_ShouldBeSuccessful()
    {
        // Arrange
        var rowLevelAccess = RepositoryTestsHelper.CreateRowLevelAccess();
        rowLevelAccess.UserId = _user.UserId;
        await _unitOfWork.RowLevelAccessRepository.Add(rowLevelAccess);
        
        // Act
        var result = await _unitOfWork.RowLevelAccessRepository.GetByUserId(rowLevelAccess.UserId);

        // Assert
        var rowLevelAccesses = result as RowLevelAccess[] ?? result.ToArray();
        Assert.NotEmpty(rowLevelAccesses);
        Assert.All(rowLevelAccesses, r => Assert.Equal(rowLevelAccess.UserId, r.UserId));
    }
}