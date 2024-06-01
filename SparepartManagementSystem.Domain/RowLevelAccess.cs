using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class RowLevelAccess : BaseModel
{
    private int _rowLevelAccessId;
    public int RowLevelAccessId
    {
        get => _rowLevelAccessId;
        set
        {
            if (_rowLevelAccessId == value)
            {
                return;
            }

            _rowLevelAccessId = value;
            IsChanged = true;
        }
    }
    
    private int _userId;
    public int UserId
    {
        get => _userId;
        set
        {
            if (_userId == value)
            {
                return;
            }

            _userId = value;
            IsChanged = true;
        }
    }

    private AxTable _axTable = AxTable.None;
    public AxTable AxTable
    {
        get => _axTable;
        set
        {
            if (_axTable == value)
            {
                return;
            }

            _axTable = value;
            IsChanged = true;
        }
    }
    
    private string _query = string.Empty;
    public string Query
    {
        get => _query;
        set
        {
            if (_query == value)
            {
                return;
            }

            _query = value;
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
        
        OriginalValues[nameof(RowLevelAccessId)] = _rowLevelAccessId;
        OriginalValues[nameof(UserId)] = _userId;
        OriginalValues[nameof(AxTable)] = _axTable;
        OriginalValues[nameof(Query)] = _query;
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
        
        _rowLevelAccessId = OriginalValues[nameof(RowLevelAccessId)] as int? ?? 0;
        _userId = OriginalValues[nameof(UserId)] as int? ?? 0;
        _axTable = OriginalValues[nameof(AxTable)] as AxTable? ?? AxTable.None; 
        _query = OriginalValues[nameof(Query)] as string ?? string.Empty;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not RowLevelAccess value)
        {
            return;
        }

        RowLevelAccessId = value.RowLevelAccessId;
        UserId = value.UserId;
        AxTable = value.AxTable;
        Query = value.Query;
        CreatedBy = value.CreatedBy;
        CreatedDateTime = value.CreatedDateTime;
        ModifiedBy = value.ModifiedBy;
        ModifiedDateTime = value.ModifiedDateTime;
    }
}