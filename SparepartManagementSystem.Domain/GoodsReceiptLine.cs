using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class GoodsReceiptLine : BaseModel
{
    private int _goodsReceiptLineId;
    public int GoodsReceiptLineId
    {
        get => _goodsReceiptLineId;
        set
        {
            if (_goodsReceiptLineId == value)
            {
                return;
            }

            _goodsReceiptLineId = value;
            IsChanged = true;
        }
    }
    
    private int _goodsReceiptHeaderId;
    public int GoodsReceiptHeaderId
    {
        get => _goodsReceiptHeaderId;
        set
        {
            if (_goodsReceiptHeaderId == value)
            {
                return;
            }

            _goodsReceiptHeaderId = value;
            IsChanged = true;
        }
    }
    
    private string _itemId = string.Empty;
    public string ItemId
    {
        get => _itemId;
        set
        {
            if (_itemId == value)
            {
                return;
            }

            _itemId = value;
            IsChanged = true;
        }
    }
    
    private int _lineNumber;
    public int LineNumber
    {
        get => _lineNumber;
        set
        {
            if (_lineNumber == value)
            {
                return;
            }

            _lineNumber = value;
            IsChanged = true;
        }
    }
    
    private string _itemName = string.Empty;
    public string ItemName
    {
        get => _itemName;
        set
        {
            if (_itemName == value)
            {
                return;
            }

            _itemName = value;
            IsChanged = true;
        }
    }
    
    private ProductType _productType;
    public ProductType ProductType
    {
        get => _productType;
        set
        {
            if (_productType == value)
            {
                return;
            }

            _productType = value;
            IsChanged = true;
        }
    }
    
    private decimal _remainPurchPhysical;
    public decimal RemainPurchPhysical
    {
        get => _remainPurchPhysical;
        set
        {
            if (_remainPurchPhysical == value)
            {
                return;
            }

            _remainPurchPhysical = value;
            IsChanged = true;
        }
    }
    
    private decimal _receiveNow;
    public decimal ReceiveNow
    {
        get => _receiveNow;
        set
        {
            if (_receiveNow == value)
            {
                return;
            }

            _receiveNow = value;
            IsChanged = true;
        }
    }
    
    private decimal _purchQty;
    public decimal PurchQty
    {
        get => _purchQty;
        set
        {
            if (_purchQty == value)
            {
                return;
            }

            _purchQty = value;
            IsChanged = true;
        }
    }
    
    private string _purchUnit = string.Empty;
    public string PurchUnit
    {
        get => _purchUnit;
        set
        {
            if (_purchUnit == value)
            {
                return;
            }

            _purchUnit = value;
            IsChanged = true;
        }
    }
    
    private decimal _purchPrice;
    public decimal PurchPrice
    {
        get => _purchPrice;
        set
        {
            if (_purchPrice == value)
            {
                return;
            }

            _purchPrice = value;
            IsChanged = true;
        }
    }
    
    private decimal _lineAmount;
    public decimal LineAmount
    {
        get => _lineAmount;
        set
        {
            if (_lineAmount == value)
            {
                return;
            }

            _lineAmount = value;
            IsChanged = true;
        }
    }
    
    private string _inventLocationId = string.Empty;
    public string InventLocationId
    {
        get => _inventLocationId;
        set
        {
            if (_inventLocationId == value)
            {
                return;
            }

            _inventLocationId = value;
            IsChanged = true;
        }
    }
    
    private string _wmsLocationId = string.Empty;
    public string WMSLocationId
    {
        get => _wmsLocationId;
        set
        {
            if (_wmsLocationId == value)
            {
                return;
            }

            _wmsLocationId = value;
            IsChanged = true;
        }
    }
    
    private string _createdBy = string.Empty;
    public string CreatedBy
    {
        get => _createdBy;
        set
        {
            if (_createdBy == value)
            {
                return;
            }

            _createdBy = value;
            IsChanged = true;
        }
    }
    
    private DateTime _createdDateTime = SqlDateTime.MinValue.Value;
    public DateTime CreatedDateTime
    {
        get => _createdDateTime;
        set
        {
            if (_createdDateTime == value)
            {
                return;
            }

            _createdDateTime = value;
            IsChanged = true;
        }
    }
    
    private string _modifiedBy = string.Empty;
    public string ModifiedBy
    {
        get => _modifiedBy;
        set
        {
            if (_modifiedBy == value)
            {
                return;
            }

            _modifiedBy = value;
            IsChanged = true;
        }
    }
    
    private DateTime _modifiedDateTime = SqlDateTime.MinValue.Value;
    public DateTime ModifiedDateTime
    {
        get => _modifiedDateTime;
        set
        {
            if (_modifiedDateTime == value)
            {
                return;
            }

            _modifiedDateTime = value;
            IsChanged = true;
        }
    }

    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(GoodsReceiptLineId)] = _goodsReceiptLineId;
        OriginalValues[nameof(GoodsReceiptHeaderId)] = _goodsReceiptHeaderId;
        OriginalValues[nameof(ItemId)] = _itemId;
        OriginalValues[nameof(LineNumber)] = _lineNumber;
        OriginalValues[nameof(ItemName)] = _itemName;
        OriginalValues[nameof(ProductType)] = _productType;
        OriginalValues[nameof(RemainPurchPhysical)] = _remainPurchPhysical;
        OriginalValues[nameof(ReceiveNow)] = _receiveNow;
        OriginalValues[nameof(PurchQty)] = _purchQty;
        OriginalValues[nameof(PurchUnit)] = _purchUnit;
        OriginalValues[nameof(PurchPrice)] = _purchPrice;
        OriginalValues[nameof(LineAmount)] = _lineAmount;
        OriginalValues[nameof(InventLocationId)] = _inventLocationId;
        OriginalValues[nameof(WMSLocationId)] = _wmsLocationId;
        OriginalValues[nameof(CreatedBy)] = _createdBy;
        OriginalValues[nameof(CreatedDateTime)] = _createdDateTime;
        OriginalValues[nameof(ModifiedBy)] = _modifiedBy;
        OriginalValues[nameof(ModifiedDateTime)] = _modifiedDateTime;
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        _goodsReceiptLineId = OriginalValues[nameof(GoodsReceiptLineId)] as int? ?? 0;
        _goodsReceiptHeaderId = OriginalValues[nameof(GoodsReceiptHeaderId)] as int? ?? 0;
        _itemId = OriginalValues[nameof(ItemId)] as string ?? string.Empty;
        _lineNumber = OriginalValues[nameof(LineNumber)] as int? ?? 0;
        _itemName = OriginalValues[nameof(ItemName)] as string ?? string.Empty;
        _productType = OriginalValues[nameof(ProductType)] as ProductType? ?? ProductType.None;
        _remainPurchPhysical = OriginalValues[nameof(RemainPurchPhysical)] as decimal? ?? 0;
        _receiveNow = OriginalValues[nameof(ReceiveNow)] as decimal? ?? 0;
        _purchQty = OriginalValues[nameof(PurchQty)] as decimal? ?? 0;
        _purchUnit = OriginalValues[nameof(PurchUnit)] as string ?? string.Empty;
        _purchPrice = OriginalValues[nameof(PurchPrice)] as decimal? ?? 0;
        _lineAmount = OriginalValues[nameof(LineAmount)] as decimal? ?? 0;
        _inventLocationId = OriginalValues[nameof(InventLocationId)] as string ?? string.Empty;
        _wmsLocationId = OriginalValues[nameof(WMSLocationId)] as string ?? string.Empty;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not GoodsReceiptLine value)
        {
            return;
        }

        GoodsReceiptLineId = value.GoodsReceiptLineId;
        GoodsReceiptHeaderId = value.GoodsReceiptHeaderId;
        ItemId = value.ItemId;
        LineNumber = value.LineNumber;
        ItemName = value.ItemName;
        ProductType = value.ProductType;
        RemainPurchPhysical = value.RemainPurchPhysical;
        ReceiveNow = value.ReceiveNow;
        PurchQty = value.PurchQty;
        PurchUnit = value.PurchUnit;
        PurchPrice = value.PurchPrice;
        LineAmount = value.LineAmount;
        InventLocationId = value.InventLocationId;
        WMSLocationId = value.WMSLocationId;
        CreatedBy = value.CreatedBy;
        CreatedDateTime = value.CreatedDateTime;
        ModifiedBy = value.ModifiedBy;
        ModifiedDateTime = value.ModifiedDateTime;
    }
}