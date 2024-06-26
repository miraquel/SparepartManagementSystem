using System.Globalization;
using JetBrains.Annotations;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IGoodsReceiptLineRepository))]
public class GoodsReceiptLineRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IUnitOfWork _unitOfWork;
    private readonly GoodsReceiptHeader _goodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();

    public GoodsReceiptLineRepositoryTest()
    {
        _unitOfWork = _serviceCollectionHelper.GetRequiredService<IUnitOfWork>();
    }
    
    public async Task InitializeAsync()
    {
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(_goodsReceiptHeader);
        _goodsReceiptHeader.GoodsReceiptHeaderId = await _unitOfWork.GetLastInsertedId();
    }

    public async Task DisposeAsync()
    {
        await _unitOfWork.Rollback();
    }
    
    [Fact]
    public async Task Add_ShouldReturnGoodsReceiptLine()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        
        // Act
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        
        // Assert
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(await _unitOfWork.GetLastInsertedId());
        Assert.True(result.GoodsReceiptLineId > 0);
        Assert.Equal(goodsReceiptLine.GoodsReceiptHeaderId, result.GoodsReceiptHeaderId);
        Assert.Equal(goodsReceiptLine.ItemId, result.ItemId);
        Assert.Equal(goodsReceiptLine.CreatedBy, result.CreatedBy);
        Assert.Equal(goodsReceiptLine.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(goodsReceiptLine.ModifiedBy, result.ModifiedBy);
        Assert.Equal(goodsReceiptLine.ModifiedDateTime, result.ModifiedDateTime);
        Assert.Equal(goodsReceiptLine.ItemName, result.ItemName);
        Assert.Equal(goodsReceiptLine.LineAmount, result.LineAmount);
        Assert.Equal(goodsReceiptLine.LineNumber, result.LineNumber);
        Assert.Equal(goodsReceiptLine.ProductType, result.ProductType);
        Assert.Equal(goodsReceiptLine.PurchPrice, result.PurchPrice);
        Assert.Equal(goodsReceiptLine.PurchQty, result.PurchQty);
        Assert.Equal(goodsReceiptLine.PurchUnit, result.PurchUnit);
        Assert.Equal(goodsReceiptLine.ReceiveNow, result.ReceiveNow);
        Assert.Equal(goodsReceiptLine.InventLocationId, result.InventLocationId);
        Assert.Equal(goodsReceiptLine.RemainPurchPhysical, result.RemainPurchPhysical);
        Assert.Equal(goodsReceiptLine.WMSLocationId, result.WMSLocationId);
    }
    
    [Fact]
    public async Task Add_ShouldThrowExceptionWhenEntityIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<MySqlException>(() => _unitOfWork.GoodsReceiptLineRepository.Add(null!));
    }
    
    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        await _unitOfWork.GoodsReceiptLineRepository.Delete(id);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptLineRepository.GetById(id));
    }
    
    [Fact]
    public async Task Delete_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptLineRepository.Delete(9999));
    }
    
    [Fact]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        var id = await _unitOfWork.GetLastInsertedId();
        var anotherGoodsReceiptHeader = RepositoryTestsHelper.CreateGoodsReceiptHeader();
        await _unitOfWork.GoodsReceiptHeaderRepository.Add(anotherGoodsReceiptHeader);
        var anotherGoodsReceiptHeaderId = await _unitOfWork.GetLastInsertedId();
        var productTypes = Enum.GetValues<ProductType>().Where(x => x != goodsReceiptLine.ProductType).ToArray();
        var updatedGoodsReceiptLine = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        updatedGoodsReceiptLine.GoodsReceiptLineId = id;
        updatedGoodsReceiptLine.GoodsReceiptHeaderId = anotherGoodsReceiptHeaderId;
        updatedGoodsReceiptLine.ItemId = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.ItemName = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.LineAmount = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.LineNumber = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.ProductType = productTypes[RepositoryTestsHelper.Random.Next(productTypes.Length)];
        updatedGoodsReceiptLine.PurchPrice = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.PurchQty = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.PurchUnit = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.ReceiveNow = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.InventLocationId = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.RemainPurchPhysical = RepositoryTestsHelper.Random.Next();
        updatedGoodsReceiptLine.WMSLocationId = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.CreatedBy = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.CreatedDateTime = DateTime.Now.AddDays(1).TrimMiliseconds();
        updatedGoodsReceiptLine.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.ModifiedDateTime = DateTime.Now.AddDays(1).TrimMiliseconds();
        
        // Act
        await _unitOfWork.GoodsReceiptLineRepository.Update(updatedGoodsReceiptLine);
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        
        // Assert
        Assert.Equal(updatedGoodsReceiptLine.GoodsReceiptHeaderId, result.GoodsReceiptHeaderId);
        Assert.Equal(updatedGoodsReceiptLine.ItemId, result.ItemId);
        Assert.Equal(updatedGoodsReceiptLine.ItemName, result.ItemName);
        Assert.Equal(updatedGoodsReceiptLine.LineAmount, result.LineAmount);
        Assert.Equal(updatedGoodsReceiptLine.LineNumber, result.LineNumber);
        Assert.Equal(updatedGoodsReceiptLine.ProductType, result.ProductType);
        Assert.Equal(updatedGoodsReceiptLine.PurchPrice, result.PurchPrice);
        Assert.Equal(updatedGoodsReceiptLine.PurchQty, result.PurchQty);
        Assert.Equal(updatedGoodsReceiptLine.PurchUnit, result.PurchUnit);
        Assert.Equal(updatedGoodsReceiptLine.ReceiveNow, result.ReceiveNow);
        Assert.Equal(updatedGoodsReceiptLine.InventLocationId, result.InventLocationId);
        Assert.Equal(updatedGoodsReceiptLine.RemainPurchPhysical, result.RemainPurchPhysical);
        Assert.Equal(updatedGoodsReceiptLine.WMSLocationId, result.WMSLocationId);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedDateTime, result.ModifiedDateTime);
        Assert.NotEqual(updatedGoodsReceiptLine.CreatedBy, result.CreatedBy);
        Assert.NotEqual(updatedGoodsReceiptLine.CreatedDateTime, result.CreatedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateEntityWhenThereAreNoChanges()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine, RepositoryTestsHelper.OnBeforeAdd);
        var id = await _unitOfWork.GetLastInsertedId();
        goodsReceiptLine.GoodsReceiptLineId = id;
        var updatedGoodsReceiptLine = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        updatedGoodsReceiptLine.ModifiedBy = RepositoryTestsHelper.RandomString(12);
        updatedGoodsReceiptLine.ModifiedBy = goodsReceiptLine.ModifiedBy;
        
        // Act
        await _unitOfWork.GoodsReceiptLineRepository.Update(updatedGoodsReceiptLine);
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        
        // Assert
        Assert.Equal(updatedGoodsReceiptLine.GoodsReceiptLineId, result.GoodsReceiptLineId);
        Assert.Equal(updatedGoodsReceiptLine.GoodsReceiptHeaderId, result.GoodsReceiptHeaderId);
        Assert.Equal(updatedGoodsReceiptLine.ItemId, result.ItemId);
        Assert.Equal(updatedGoodsReceiptLine.ItemName, result.ItemName);
        Assert.Equal(updatedGoodsReceiptLine.LineAmount, result.LineAmount);
        Assert.Equal(updatedGoodsReceiptLine.LineNumber, result.LineNumber);
        Assert.Equal(updatedGoodsReceiptLine.ProductType, result.ProductType);
        Assert.Equal(updatedGoodsReceiptLine.PurchPrice, result.PurchPrice);
        Assert.Equal(updatedGoodsReceiptLine.PurchQty, result.PurchQty);
        Assert.Equal(updatedGoodsReceiptLine.PurchUnit, result.PurchUnit);
        Assert.Equal(updatedGoodsReceiptLine.ReceiveNow, result.ReceiveNow);
        Assert.Equal(updatedGoodsReceiptLine.InventLocationId, result.InventLocationId);
        Assert.Equal(updatedGoodsReceiptLine.RemainPurchPhysical, result.RemainPurchPhysical);
        Assert.Equal(updatedGoodsReceiptLine.WMSLocationId, result.WMSLocationId);
        Assert.Equal(updatedGoodsReceiptLine.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedGoodsReceiptLine.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedDateTime, result.ModifiedDateTime);
    }
    
    [Fact]
    public async Task Update_ShouldThrowExceptionWhenEntityOriginalValuesAreEmpty()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine, RepositoryTestsHelper.OnBeforeAdd);
        var id = await _unitOfWork.GetLastInsertedId();
        goodsReceiptLine.GoodsReceiptLineId = id;
        var updatedGoodsReceiptLine = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        
        // Act
        await _unitOfWork.GoodsReceiptLineRepository.Update(updatedGoodsReceiptLine);
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        
        // Assert
        Assert.Equal(updatedGoodsReceiptLine.GoodsReceiptLineId, result.GoodsReceiptLineId);
        Assert.Equal(updatedGoodsReceiptLine.GoodsReceiptHeaderId, result.GoodsReceiptHeaderId);
        Assert.Equal(updatedGoodsReceiptLine.ItemId, result.ItemId);
        Assert.Equal(updatedGoodsReceiptLine.ItemName, result.ItemName);
        Assert.Equal(updatedGoodsReceiptLine.LineAmount, result.LineAmount);
        Assert.Equal(updatedGoodsReceiptLine.LineNumber, result.LineNumber);
        Assert.Equal(updatedGoodsReceiptLine.ProductType, result.ProductType);
        Assert.Equal(updatedGoodsReceiptLine.PurchPrice, result.PurchPrice);
        Assert.Equal(updatedGoodsReceiptLine.PurchQty, result.PurchQty);
        Assert.Equal(updatedGoodsReceiptLine.PurchUnit, result.PurchUnit);
        Assert.Equal(updatedGoodsReceiptLine.ReceiveNow, result.ReceiveNow);
        Assert.Equal(updatedGoodsReceiptLine.InventLocationId, result.InventLocationId);
        Assert.Equal(updatedGoodsReceiptLine.RemainPurchPhysical, result.RemainPurchPhysical);
        Assert.Equal(updatedGoodsReceiptLine.WMSLocationId, result.WMSLocationId);
        Assert.Equal(updatedGoodsReceiptLine.CreatedBy, result.CreatedBy);
        Assert.Equal(updatedGoodsReceiptLine.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedBy, result.ModifiedBy);
        Assert.Equal(updatedGoodsReceiptLine.ModifiedDateTime, result.ModifiedDateTime);
    }

    [Fact]
    public async Task Update_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        goodsReceiptLine.AcceptChanges();
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptLineRepository.Update(goodsReceiptLine, RepositoryTestsHelper.OnBeforeUpdate));
    }
    
    [Fact]
    public async Task GetById_ShouldReturnEntity()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        var id = await _unitOfWork.GetLastInsertedId();
        
        // Act
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);
        
        // Assert
        Assert.Equal(goodsReceiptLine.GoodsReceiptHeaderId, result.GoodsReceiptHeaderId);
        Assert.Equal(goodsReceiptLine.ItemId, result.ItemId);
        Assert.Equal(goodsReceiptLine.ItemName, result.ItemName);
        Assert.Equal(goodsReceiptLine.LineAmount, result.LineAmount);
        Assert.Equal(goodsReceiptLine.LineNumber, result.LineNumber);
        Assert.Equal(goodsReceiptLine.ProductType, result.ProductType);
        Assert.Equal(goodsReceiptLine.PurchPrice, result.PurchPrice);
        Assert.Equal(goodsReceiptLine.PurchQty, result.PurchQty);
        Assert.Equal(goodsReceiptLine.PurchUnit, result.PurchUnit);
        Assert.Equal(goodsReceiptLine.ReceiveNow, result.ReceiveNow);
        Assert.Equal(goodsReceiptLine.InventLocationId, result.InventLocationId);
        Assert.Equal(goodsReceiptLine.RemainPurchPhysical, result.RemainPurchPhysical);
        Assert.Equal(goodsReceiptLine.WMSLocationId, result.WMSLocationId);
        Assert.Equal(goodsReceiptLine.CreatedBy, result.CreatedBy);
        Assert.Equal(goodsReceiptLine.CreatedDateTime, result.CreatedDateTime);
        Assert.Equal(goodsReceiptLine.ModifiedBy, result.ModifiedBy);
        Assert.Equal(goodsReceiptLine.ModifiedDateTime, result.ModifiedDateTime);
    }

    [Fact]
    public async Task GetById_ShouldThrowExceptionWhenEntityDoesNotExist()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.GoodsReceiptLineRepository.GetById(9999));
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        
        // Act
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetAll();
        
        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetByParams_ShouldReturnEntities()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        goodsReceiptLine.GoodsReceiptLineId = await _unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            { "goodsReceiptLineId", goodsReceiptLine.GoodsReceiptLineId.ToString() },
            { "itemId", goodsReceiptLine.ItemId },
            { "goodsReceiptHeaderId", goodsReceiptLine.GoodsReceiptHeaderId.ToString() },
            { "productType", goodsReceiptLine.ProductType.ToString() },
            { "inventLocationId", goodsReceiptLine.InventLocationId },
            { "wmsLocationId", goodsReceiptLine.WMSLocationId },
            { "createdBy", goodsReceiptLine.CreatedBy },
            { "modifiedBy", goodsReceiptLine.ModifiedBy },
            { "itemName", goodsReceiptLine.ItemName },
            { "purchUnit", goodsReceiptLine.PurchUnit },
            { "lineAmount", goodsReceiptLine.LineAmount.ToString(CultureInfo.InvariantCulture) },
            { "lineNumber", goodsReceiptLine.LineNumber.ToString() },
            { "purchPrice", goodsReceiptLine.PurchPrice.ToString(CultureInfo.InvariantCulture) },
            { "purchQty", goodsReceiptLine.PurchQty.ToString(CultureInfo.InvariantCulture) },
            { "receiveNow", goodsReceiptLine.ReceiveNow.ToString(CultureInfo.InvariantCulture) },
            { "remainPurchPhysical", goodsReceiptLine.RemainPurchPhysical.ToString(CultureInfo.InvariantCulture) },
            { "createdDateTime", goodsReceiptLine.CreatedDateTime.ToString(CultureInfo.InvariantCulture) },
            { "modifiedDateTime", goodsReceiptLine.ModifiedDateTime.ToString(CultureInfo.InvariantCulture) }
        };
        
        // Act
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetByParams(parameters);
        
        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetByGoodsReceiptHeaderId_ShouldReturnEntities()
    {
        // Arrange
        var goodsReceiptLine = RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId);
        await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLine);
        
        // Act
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetByGoodsReceiptHeaderId(_goodsReceiptHeader.GoodsReceiptHeaderId);
        
        // Assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task BulkAdd_ShouldReturnEntities()
    {
        // Arrange
        var goodsReceiptLines = new List<GoodsReceiptLine>
        {
            RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId),
            RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId)
        };

        // Act
        await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(goodsReceiptLines);
        
        // Assert
        var result = await _unitOfWork.GoodsReceiptLineRepository.GetAll();
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task BulkAdd_ShouldThrowExceptionWhenEntitiesIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _unitOfWork.GoodsReceiptLineRepository.BulkAdd(null!));
    }
    
    [Fact]
    public async Task BulkAdd_ShouldThrowExceptionWhenIdIsDuplicate()
    {
        // Arrange
        var goodsReceiptLines = new List<GoodsReceiptLine>
        {
            RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId),
            RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId),
            RepositoryTestsHelper.CreateGoodsReceiptLine(_goodsReceiptHeader.GoodsReceiptHeaderId)
        };
        foreach (var goodsReceiptLine in goodsReceiptLines)
        {
            goodsReceiptLine.GoodsReceiptLineId = 1;
        }

        // Act & Assert
        await Assert.ThrowsAsync<MySqlException>(() => _unitOfWork.GoodsReceiptLineRepository.BulkAdd(goodsReceiptLines, InfoMessageEventHandler));
        
        return;

        void InfoMessageEventHandler(object _, object args)
        {
            if (args is not MySqlInfoMessageEventArgs infoMessageEventArgs) return;
            
            foreach (var message in infoMessageEventArgs.Errors)
            {
                Assert.Equal("Duplicate entry '1' for key 'goodsreceiptlines.PRIMARY'", message.Message);
            }
        }
    }
}