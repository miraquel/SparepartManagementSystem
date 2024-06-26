using System.Globalization;
using JetBrains.Annotations;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IGoodsReceiptHeaderRepository))]
public class GoodsReceiptHeaderRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IUnitOfWork _unitOfWork;

    public GoodsReceiptHeaderRepositoryTest()
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
    public async Task Add_ShouldReturnGoodsReceiptHeader()
    {
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();

        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader, RepositoryTestsHelper.OnBeforeAdd);
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(await _unitOfWork.GetLastInsertedId());
        
        Assert.True(result.GoodsReceiptHeaderId > 0);
        Assert.Equal(goodsReceiptHeader.PurchId, result.PurchId);
        Assert.Equal(goodsReceiptHeader.Description, result.Description);
        Assert.Equal(goodsReceiptHeader.InvoiceAccount, result.InvoiceAccount);
        Assert.Equal(goodsReceiptHeader.OrderAccount, result.OrderAccount);
        Assert.Equal(goodsReceiptHeader.IsSubmitted, result.IsSubmitted);
        Assert.Equal(goodsReceiptHeader.SubmittedDate, result.SubmittedDate);
        Assert.Equal(goodsReceiptHeader.CreatedBy, result.CreatedBy);
        Assert.Equal(goodsReceiptHeader.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(goodsReceiptHeader.ModifiedBy, result.ModifiedBy);
        Assert.Equal(goodsReceiptHeader.ModifiedDateTime, result.ModifiedDateTime);
    }

    [Fact]
    public async Task Add_ShouldThrowExceptionWhenEntityIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<MySqlException>(() => _unitOfWork.GoodsReceiptHeaderRepository.Add(null!));
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        // Arrange
        var unitOfWork = _unitOfWork;
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await unitOfWork.GetLastInsertedId();

        // Act
        await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptHeaderRepository.GetById(id));
    }

    [Fact]
    public async Task Delete_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Arrange

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptHeaderRepository.Delete(9999));
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader, RepositoryTestsHelper.OnBeforeAdd);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedGoodsReceiptHeader = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);
        updatedGoodsReceiptHeader.UpdateProperties(RepositoryTestsHelper.CreateGoodsReceiptHeader());
        updatedGoodsReceiptHeader.IsSubmitted = !goodsReceiptHeader.IsSubmitted;
        updatedGoodsReceiptHeader.CreatedBy = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptHeader.CreatedDateTime = RepositoryTestsHelper.RandomDateTime();

        // Act
        await _unitOfWork.GoodsReceiptHeaderRepository.Update(updatedGoodsReceiptHeader, RepositoryTestsHelper.OnBeforeUpdate);
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

        // Assert
        Assert.Equal(updatedGoodsReceiptHeader.PurchId, result.PurchId);
        Assert.Equal(updatedGoodsReceiptHeader.Description, result.Description);
        Assert.Equal(updatedGoodsReceiptHeader.InvoiceAccount, result.InvoiceAccount);
        Assert.Equal(updatedGoodsReceiptHeader.OrderAccount, result.OrderAccount);
        Assert.Equal(updatedGoodsReceiptHeader.IsSubmitted, result.IsSubmitted);
        Assert.Equal(updatedGoodsReceiptHeader.SubmittedDate, result.SubmittedDate);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedDateTime, result.ModifiedDateTime);
        Assert.NotEqual(updatedGoodsReceiptHeader.CreatedBy, result.CreatedBy);
        Assert.NotEqual(updatedGoodsReceiptHeader.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotModifyEntityWhenThereAreNoChanges()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedGoodsReceiptHeader = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);
        updatedGoodsReceiptHeader.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptHeader.ModifiedBy = goodsReceiptHeader.ModifiedBy;

        // Act
        await _unitOfWork.GoodsReceiptHeaderRepository.Update(updatedGoodsReceiptHeader);
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

        // Assert
        Assert.Equal(updatedGoodsReceiptHeader.PurchId, result.PurchId);
        Assert.Equal(updatedGoodsReceiptHeader.Description, result.Description);
        Assert.Equal(updatedGoodsReceiptHeader.InvoiceAccount, result.InvoiceAccount);
        Assert.Equal(updatedGoodsReceiptHeader.OrderAccount, result.OrderAccount);
        Assert.Equal(updatedGoodsReceiptHeader.IsSubmitted, result.IsSubmitted);
        Assert.Equal(updatedGoodsReceiptHeader.SubmittedDate, result.SubmittedDate);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedGoodsReceiptHeader.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedGoodsReceiptHeader.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotModifyEntityWhenOriginalValuesAreEmpty()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedGoodsReceiptHeader = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

        // Act
        await _unitOfWork.GoodsReceiptHeaderRepository.Update(updatedGoodsReceiptHeader);
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

        // Assert
        Assert.Equal(updatedGoodsReceiptHeader.PurchId, result.PurchId);
        Assert.Equal(updatedGoodsReceiptHeader.Description, result.Description);
        Assert.Equal(updatedGoodsReceiptHeader.InvoiceAccount, result.InvoiceAccount);
        Assert.Equal(updatedGoodsReceiptHeader.OrderAccount, result.OrderAccount);
        Assert.Equal(updatedGoodsReceiptHeader.IsSubmitted, result.IsSubmitted);
        Assert.Equal(updatedGoodsReceiptHeader.SubmittedDate, result.SubmittedDate);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptHeader.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(updatedGoodsReceiptHeader.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedGoodsReceiptHeader.CreatedDateTime, result.CreatedDateTime);
    }

    [Fact]
    public async Task Update_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        goodsReceiptHeader.AcceptChanges();
        goodsReceiptHeader.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        goodsReceiptHeader.ModifiedDateTime = RepositoryTestsHelper.RandomDateTime();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptHeaderRepository.Update(goodsReceiptHeader));
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAll();

        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnEntity()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetById_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptHeaderRepository.GetById(9999));
    }
    
    [Fact]
    public async Task GetByParams_ShouldReturnEntities()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader, RepositoryTestsHelper.OnBeforeAdd);
        goodsReceiptHeader.GoodsReceiptHeaderId = await _unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            {"goodsReceiptHeaderId", goodsReceiptHeader.GoodsReceiptHeaderId.ToString()},
            {"packingSlipId", goodsReceiptHeader.PackingSlipId},
            {"purchId", goodsReceiptHeader.PurchId},
            {"description", goodsReceiptHeader.Description},
            {"invoiceAccount", goodsReceiptHeader.InvoiceAccount},
            {"orderAccount", goodsReceiptHeader.OrderAccount},
            {"purchName", goodsReceiptHeader.PurchName},
            {"purchStatus", goodsReceiptHeader.PurchStatus},
            {"submittedBy", goodsReceiptHeader.SubmittedBy},
            {"isSubmitted", goodsReceiptHeader.IsSubmitted.ToString()},
            {"submittedDate", goodsReceiptHeader.SubmittedDate.ToString(CultureInfo.InvariantCulture)},
            {"transDate", goodsReceiptHeader.TransDate.ToString(CultureInfo.InvariantCulture)},
            {"createdBy", goodsReceiptHeader.CreatedBy},
            {"createdDateTime", goodsReceiptHeader.CreatedDateTime.ToString(CultureInfo.InvariantCulture)},
            {"modifiedBy", goodsReceiptHeader.ModifiedBy},
            {"modifiedDateTime", goodsReceiptHeader.ModifiedDateTime.ToString(CultureInfo.InvariantCulture)}
        };

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParams(parameters);

        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetAllPagedList_ShouldReturnPagedEntities()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAllPagedList(1, 10);

        // Assert
        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public async Task GetByParamsPagedList_ShouldReturnPagedEntities()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader, RepositoryTestsHelper.OnBeforeAdd);
        goodsReceiptHeader.GoodsReceiptHeaderId = await _unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            {"goodsReceiptHeaderId", goodsReceiptHeader.GoodsReceiptHeaderId.ToString()},
            {"packingSlipId", goodsReceiptHeader.PackingSlipId},
            {"purchId", goodsReceiptHeader.PurchId},
            {"description", goodsReceiptHeader.Description},
            {"invoiceAccount", goodsReceiptHeader.InvoiceAccount},
            {"orderAccount", goodsReceiptHeader.OrderAccount},
            {"purchName", goodsReceiptHeader.PurchName},
            {"purchStatus", goodsReceiptHeader.PurchStatus},
            {"submittedBy", goodsReceiptHeader.SubmittedBy},
            {"isSubmitted", goodsReceiptHeader.IsSubmitted.ToString()},
            {"submittedDate", goodsReceiptHeader.SubmittedDate.ToString(CultureInfo.InvariantCulture)},
            {"transDate", goodsReceiptHeader.TransDate.ToString(CultureInfo.InvariantCulture)},
            {"createdBy", goodsReceiptHeader.CreatedBy},
            {"createdDateTime", goodsReceiptHeader.CreatedDateTime.ToString(CultureInfo.InvariantCulture)},
            {"modifiedBy", goodsReceiptHeader.ModifiedBy},
            {"modifiedDateTime", goodsReceiptHeader.ModifiedDateTime.ToString(CultureInfo.InvariantCulture)}
        };

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParamsPagedList(1, 10, parameters);

        // Assert
        Assert.NotEmpty(result.Items);
    }
    
    [Fact]
    public async Task GetByIdWithLines_ShouldReturnEntity()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await _unitOfWork.GetLastInsertedId();
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(id);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(id);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetByIdWithLines_ShouldReturnEntityWhenForUpdateIsTrue()
    {
        // Arrange
        var goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeader);
        var id = await _unitOfWork.GetLastInsertedId();
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(id);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);

        // Act
        var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(id, true);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetByIdWithLines_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(9999));
    }
}