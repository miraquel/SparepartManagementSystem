using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Shared.Helper;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(INumberSequenceRepository))]
public class NumberSequenceRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IUnitOfWork _unitOfWork;

    public NumberSequenceRepositoryTest()
    {
        _unitOfWork = _serviceCollectionHelper.GetRequiredService<IUnitOfWork>();
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _unitOfWork.Rollback();
    }
    
    [Fact]
    public async Task Add_WhenCalled_ShouldAddNumberSequence()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();

        // Act
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);

        // Assert
        var id = await _unitOfWork.GetLastInsertedId();
        numberSequence.NumberSequenceId = id;
        var result = await _unitOfWork.NumberSequenceRepository.GetById(id);
        Assert.Equal(numberSequence.NumberSequenceId, result.NumberSequenceId);
        Assert.Equal(numberSequence.Name, result.Name);
        Assert.Equal(numberSequence.Description, result.Description);
        Assert.Equal(numberSequence.Format, result.Format);
        Assert.Equal(numberSequence.LastNumber, result.LastNumber);
        Assert.Equal(numberSequence.Module, result.Module);
        Assert.Equal(numberSequence.CreatedBy, result.CreatedBy);
        Assert.Equal(numberSequence.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(numberSequence.ModifiedBy, result.ModifiedBy);
        Assert.Equal(numberSequence.ModifiedDateTime, result.ModifiedDateTime);
    }

    [Fact]
    public async Task Delete_WhenCalled_ShouldDeleteNumberSequence()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);

        // Act
        await _unitOfWork.NumberSequenceRepository.Delete(numberSequence.NumberSequenceId);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.NumberSequenceRepository.GetById(numberSequence.NumberSequenceId));
    }

    [Fact]
    public async Task GetAll_WhenCalled_ShouldReturnAllNumberSequences()
    {
        // Arrange
        var numberSequences = new List<NumberSequence>
        {
            RepositoryTestsHelper.CreateNumberSequence(),
            RepositoryTestsHelper.CreateNumberSequence(),
            RepositoryTestsHelper.CreateNumberSequence()
        };
        foreach (var numberSequence in numberSequences)
        {
            await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        }

        // Act
        var result = await _unitOfWork.NumberSequenceRepository.GetAll();

        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetById_WhenCalled_ShouldReturnNumberSequence()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        var id = await _unitOfWork.GetLastInsertedId();
        numberSequence.NumberSequenceId = id;

        // Act
        var result = await _unitOfWork.NumberSequenceRepository.GetById(id);

        // Assert
        Assert.Equal(numberSequence.NumberSequenceId, result.NumberSequenceId);
        Assert.Equal(numberSequence.Name, result.Name);
        Assert.Equal(numberSequence.Description, result.Description);
        Assert.Equal(numberSequence.Format, result.Format);
        Assert.Equal(numberSequence.LastNumber, result.LastNumber);
        Assert.Equal(numberSequence.Module, result.Module);
        Assert.Equal(numberSequence.CreatedBy, result.CreatedBy);
        Assert.Equal(numberSequence.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(numberSequence.ModifiedBy, result.ModifiedBy);
        Assert.Equal(numberSequence.ModifiedDateTime, result.ModifiedDateTime);
    }
    
    [Fact]
    public async Task GetNextNumberByModule_WhenCalled_ShouldReturnNextNumber()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        var id = await _unitOfWork.GetLastInsertedId();
        numberSequence.NumberSequenceId = id;

        // Act
        var result = await _unitOfWork.NumberSequenceRepository.GetNextNumberByModule(numberSequence.Module);

        // Assert
        var formatString = RegexHelper.NumberSequenceRegex().Matches(numberSequence.Format);
        var expected = RegexHelper.NumberSequenceRegex().Replace(numberSequence.Format,
            (numberSequence.LastNumber + 1).ToString().PadLeft(formatString[0].Length, '0'));
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task GetNextNumberByModule_EnsureLastNumberSequenceNotDuplicate()
    {
        // Arrange
        var scopeFactory = _serviceCollectionHelper.GetRequiredService<IServiceScopeFactory>();
        using var addScope = scopeFactory.CreateScope();
        var addService = ServiceCollectionHelper.GetRequiredService<IUnitOfWork>(addScope);
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await addService.NumberSequenceRepository.Add(numberSequence);
        numberSequence.NumberSequenceId = await addService.GetLastInsertedId();
        await addService.Commit();
        
        using var firstScope = scopeFactory.CreateScope();
        using var secondScope = scopeFactory.CreateScope();
        using var thirdScope = scopeFactory.CreateScope();

        var firstService = ServiceCollectionHelper.GetRequiredService<IUnitOfWork>(firstScope);
        var secondService = ServiceCollectionHelper.GetRequiredService<IUnitOfWork>(secondScope);
        var thirdService = ServiceCollectionHelper.GetRequiredService<IUnitOfWork>(thirdScope);

        // Act
        var tasks = new List<Task<string>>
        {
            Task.Run(async () =>
            {
                var result = await firstService.NumberSequenceRepository.GetNextNumberByModule(numberSequence.Module);
                await firstService.Commit();
                return result;
            }),
            Task.Run(async () =>
            {
                var result = await secondService.NumberSequenceRepository.GetNextNumberByModule(numberSequence.Module);
                await secondService.Commit();
                return result;
            }),
            Task.Run(async () =>
            {
                var result = await thirdService.NumberSequenceRepository.GetNextNumberByModule(numberSequence.Module);
                await thirdService.Commit();
                return result;
            })
        };

        var results = await Task.WhenAll(tasks);
        
        // Assert
        Assert.True(results.Distinct().Count() == results.Length);
        
        // Cleanup
        var deleteScope = scopeFactory.CreateScope();
        var deleteService = ServiceCollectionHelper.GetRequiredService<IUnitOfWork>(deleteScope);
        await deleteService.NumberSequenceRepository.Delete(numberSequence.NumberSequenceId);
        await deleteService.Commit();
    }
    
    [Fact]
    public async Task Update_WhenCalled_ShouldUpdateNumberSequence()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedNumberSequence = await _unitOfWork.NumberSequenceRepository.GetById(id);
        updatedNumberSequence.UpdateProperties(RepositoryTestsHelper.CreateNumberSequence());
        updatedNumberSequence.CreatedBy = RepositoryTestsHelper.RandomString(12);
        updatedNumberSequence.CreatedDateTime = DateTime.Now.TrimMiliseconds().AddDays(1);

        // Act
        await _unitOfWork.NumberSequenceRepository.Update(updatedNumberSequence, RepositoryTestsHelper.OnBeforeUpdate);

        // Assert
        var result = await _unitOfWork.NumberSequenceRepository.GetById(id);
        Assert.Equal(updatedNumberSequence.NumberSequenceId, result.NumberSequenceId);
        Assert.Equal(updatedNumberSequence.Name, result.Name);
        Assert.Equal(updatedNumberSequence.Description, result.Description);
        Assert.Equal(updatedNumberSequence.Format, result.Format);
        Assert.Equal(updatedNumberSequence.LastNumber, result.LastNumber);
        Assert.Equal(updatedNumberSequence.Module, result.Module);
        Assert.Equal(updatedNumberSequence.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedNumberSequence.ModifiedDateTime, result.ModifiedDateTime);
        Assert.NotEqual(updatedNumberSequence.CreatedBy, result.CreatedBy);
        Assert.NotEqual(updatedNumberSequence.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateEntityIfThereAreNoChanges()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedNumberSequence = await _unitOfWork.NumberSequenceRepository.GetById(id);
        updatedNumberSequence.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedNumberSequence.ModifiedBy = numberSequence.ModifiedBy;

        // Act
        await _unitOfWork.NumberSequenceRepository.Update(updatedNumberSequence);

        // Assert
        var result = await _unitOfWork.NumberSequenceRepository.GetById(id);
        Assert.Equal(updatedNumberSequence.NumberSequenceId, result.NumberSequenceId);
        Assert.Equal(updatedNumberSequence.Name, result.Name);
        Assert.Equal(updatedNumberSequence.Description, result.Description);
        Assert.Equal(updatedNumberSequence.Format, result.Format);
        Assert.Equal(updatedNumberSequence.LastNumber, result.LastNumber);
        Assert.Equal(updatedNumberSequence.Module, result.Module);
        Assert.Equal(updatedNumberSequence.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedNumberSequence.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedNumberSequence.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedNumberSequence.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateEntityIfOriginalValuesAreEmpty()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedNumberSequence = await _unitOfWork.NumberSequenceRepository.GetById(id);

        // Act
        await _unitOfWork.NumberSequenceRepository.Update(updatedNumberSequence);

        // Assert
        var result = await _unitOfWork.NumberSequenceRepository.GetById(id);
        Assert.Equal(updatedNumberSequence.NumberSequenceId, result.NumberSequenceId);
        Assert.Equal(updatedNumberSequence.Name, result.Name);
        Assert.Equal(updatedNumberSequence.Description, result.Description);
        Assert.Equal(updatedNumberSequence.Format, result.Format);
        Assert.Equal(updatedNumberSequence.LastNumber, result.LastNumber);
        Assert.Equal(updatedNumberSequence.Module, result.Module);
        Assert.Equal(updatedNumberSequence.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedNumberSequence.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedNumberSequence.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedNumberSequence.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_WhenCalledWithNonExistingNumberSequence_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        numberSequence.AcceptChanges();
        numberSequence.Name = RepositoryTestsHelper.RandomString(12);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.NumberSequenceRepository.Update(numberSequence));
    }
    
    [Fact]
    public async Task GetByParams_WhenCalled_ShouldReturnNumberSequences()
    {
        // Arrange
        var numberSequence = RepositoryTestsHelper.CreateNumberSequence();
        await _unitOfWork.NumberSequenceRepository.Add(numberSequence, RepositoryTestsHelper.OnBeforeAdd);
        numberSequence.NumberSequenceId = await _unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            { "numberSequenceId", numberSequence.NumberSequenceId.ToString() },
            { "name", numberSequence.Name },
            { "description", numberSequence.Description },
            { "format", numberSequence.Format },
            { "lastNumber", numberSequence.LastNumber.ToString() },
            { "module", numberSequence.Module },
            { "createdBy", numberSequence.CreatedBy },
            { "createdDateTime", numberSequence.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
            { "modifiedBy", numberSequence.ModifiedBy },
            { "modifiedDateTime", numberSequence.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) }
        };

        // Act
        var result = await _unitOfWork.NumberSequenceRepository.GetByParams(parameters);

        // Assert
        Assert.NotEmpty(result);
    }
}