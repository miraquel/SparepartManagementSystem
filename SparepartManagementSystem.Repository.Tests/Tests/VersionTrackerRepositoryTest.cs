using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IVersionTrackerRepository))]
public class VersionTrackerRepositoryTest
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public VersionTrackerRepositoryTest()
    {
        var serviceProvider = new ServiceCollectionHelper();
        _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
    }
    
    [Fact]
    public async Task Add_ShouldAddVersionTracker()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var versionTracker = RepositoryTestsHelper.CreateVersionTracker();

            // Act
            await unitOfWork.VersionTrackerRepository.Add(versionTracker);
            versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();

            // Assert
            var result = await unitOfWork.VersionTrackerRepository.GetById(versionTracker.VersionTrackerId);
            Assert.Equal(versionTracker.VersionTrackerId, result.VersionTrackerId);
            Assert.Equal(versionTracker.Version, result.Version);
            Assert.Equal(versionTracker.Description, result.Description);
            Assert.Equal(versionTracker.PhysicalLocation, result.PhysicalLocation);
            Assert.Equal(versionTracker.PublishedDateTime, result.PublishedDateTime);
            Assert.Equal(versionTracker.Sha1Checksum, result.Sha1Checksum);
            Assert.Equal(versionTracker.CreatedBy, result.CreatedBy);
            Assert.Equal(versionTracker.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(versionTracker.ModifiedBy, result.ModifiedBy);
            Assert.Equal(versionTracker.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Delete_ShouldDeleteVersionTracker()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var versionTracker = RepositoryTestsHelper.CreateVersionTracker();
            await unitOfWork.VersionTrackerRepository.Add(versionTracker);
            versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();

            // Act
            await unitOfWork.VersionTrackerRepository.Delete(versionTracker.VersionTrackerId);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await unitOfWork.VersionTrackerRepository.GetById(versionTracker.VersionTrackerId));
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllVersionTrackers()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            VersionTracker[] versionTrackers =
            [
                RepositoryTestsHelper.CreateVersionTracker(),
                RepositoryTestsHelper.CreateVersionTracker(),
                RepositoryTestsHelper.CreateVersionTracker()
            ];
            
            foreach (var versionTracker in versionTrackers)
            {
                await unitOfWork.VersionTrackerRepository.Add(versionTracker);
                versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();
            }

            // Act
            var result = await unitOfWork.VersionTrackerRepository.GetAll();

            // Assert
            var versionTrackersResult = result as VersionTracker[] ?? result.ToArray();
            Assert.Equal(versionTrackers.Length, versionTrackersResult.Length);
            foreach (var versionTracker in versionTrackers)
            {
                var resultVersionTracker = versionTrackersResult.Single(x => x.VersionTrackerId == versionTracker.VersionTrackerId);
                Assert.Equal(versionTracker.VersionTrackerId, resultVersionTracker.VersionTrackerId);
                Assert.Equal(versionTracker.Version, resultVersionTracker.Version);
                Assert.Equal(versionTracker.Description, resultVersionTracker.Description);
                Assert.Equal(versionTracker.PhysicalLocation, resultVersionTracker.PhysicalLocation);
                Assert.Equal(versionTracker.PublishedDateTime, resultVersionTracker.PublishedDateTime);
                Assert.Equal(versionTracker.Sha1Checksum, resultVersionTracker.Sha1Checksum);
                Assert.Equal(versionTracker.CreatedBy, resultVersionTracker.CreatedBy);
                Assert.Equal(versionTracker.CreatedDateTime, resultVersionTracker.CreatedDateTime);
                Assert.Equal(versionTracker.ModifiedBy, resultVersionTracker.ModifiedBy);
                Assert.Equal(versionTracker.ModifiedDateTime, resultVersionTracker.ModifiedDateTime);
            }
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task GetById_ShouldReturnVersionTracker()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var versionTracker = RepositoryTestsHelper.CreateVersionTracker();
            await unitOfWork.VersionTrackerRepository.Add(versionTracker);
            versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();

            // Act
            var result = await unitOfWork.VersionTrackerRepository.GetById(versionTracker.VersionTrackerId);

            // Assert
            Assert.Equal(versionTracker.VersionTrackerId, result.VersionTrackerId);
            Assert.Equal(versionTracker.Version, result.Version);
            Assert.Equal(versionTracker.Description, result.Description);
            Assert.Equal(versionTracker.PhysicalLocation, result.PhysicalLocation);
            Assert.Equal(versionTracker.PublishedDateTime, result.PublishedDateTime);
            Assert.Equal(versionTracker.Sha1Checksum, result.Sha1Checksum);
            Assert.Equal(versionTracker.CreatedBy, result.CreatedBy);
            Assert.Equal(versionTracker.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(versionTracker.ModifiedBy, result.ModifiedBy);
            Assert.Equal(versionTracker.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldUpdateVersionTracker()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var versionTracker = RepositoryTestsHelper.CreateVersionTracker();
            await unitOfWork.VersionTrackerRepository.Add(versionTracker, RepositoryTestsHelper.OnBeforeAdd);
            versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();
            var updatedVersionTracker = await unitOfWork.VersionTrackerRepository.GetById(versionTracker.VersionTrackerId);
            updatedVersionTracker.UpdateProperties(RepositoryTestsHelper.CreateVersionTracker());

            // Act
            await unitOfWork.VersionTrackerRepository.Update(updatedVersionTracker, RepositoryTestsHelper.OnBeforeUpdate);

            // Assert
            var result = await unitOfWork.VersionTrackerRepository.GetById(updatedVersionTracker.VersionTrackerId);
            Assert.Equal(updatedVersionTracker.VersionTrackerId, result.VersionTrackerId);
            Assert.Equal(updatedVersionTracker.Version, result.Version);
            Assert.Equal(updatedVersionTracker.Description, result.Description);
            Assert.Equal(updatedVersionTracker.PhysicalLocation, result.PhysicalLocation);
            Assert.Equal(updatedVersionTracker.PublishedDateTime, result.PublishedDateTime);
            Assert.Equal(updatedVersionTracker.Sha1Checksum, result.Sha1Checksum);
            Assert.Equal(updatedVersionTracker.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedVersionTracker.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedVersionTracker.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedVersionTracker.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateWhenThereAreNoChanges()
    {
        // Setup
        using var scope = _serviceScopeFactory.CreateScope();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        try
        {
            // Arrange
            var versionTracker = RepositoryTestsHelper.CreateVersionTracker();
            await unitOfWork.VersionTrackerRepository.Add(versionTracker);
            versionTracker.VersionTrackerId = await unitOfWork.GetLastInsertedId();
            var updatedVersionTracker = await unitOfWork.VersionTrackerRepository.GetById(versionTracker.VersionTrackerId);

            // Act
            await unitOfWork.VersionTrackerRepository.Update(updatedVersionTracker);

            // Assert
            var result = await unitOfWork.VersionTrackerRepository.GetById(updatedVersionTracker.VersionTrackerId);
            Assert.Equal(updatedVersionTracker.VersionTrackerId, result.VersionTrackerId);
            Assert.Equal(updatedVersionTracker.Version, result.Version);
            Assert.Equal(updatedVersionTracker.Description, result.Description);
            Assert.Equal(updatedVersionTracker.PhysicalLocation, result.PhysicalLocation);
            Assert.Equal(updatedVersionTracker.PublishedDateTime, result.PublishedDateTime);
            Assert.Equal(updatedVersionTracker.Sha1Checksum, result.Sha1Checksum);
            Assert.Equal(updatedVersionTracker.CreatedBy, result.CreatedBy);
            Assert.Equal(updatedVersionTracker.CreatedDateTime, result.CreatedDateTime);
            Assert.Equal(updatedVersionTracker.ModifiedBy, result.ModifiedBy);
            Assert.Equal(updatedVersionTracker.ModifiedDateTime, result.ModifiedDateTime);
        }
        finally
        {
            // Cleanup
            await unitOfWork.Rollback();
        }
    }
}