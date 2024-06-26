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
        base.AcceptChanges();
        
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
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not RowLevelAccess value)
        {
            return;
        }
        
        UserId = value.UserId;
        AxTable = value.AxTable;
        Query = value.Query;
    }
}