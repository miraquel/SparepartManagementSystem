using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class WorkOrderHeader : BaseModel
{
    private int _workOrderHeaderId;
    public int WorkOrderHeaderId
    {
        get => _workOrderHeaderId;
        set
        {
            if (_workOrderHeaderId == value)
            {
                return;
            }

            _workOrderHeaderId = value;
            IsChanged = true;
        }
    }
    
    private bool _isSubmitted;
    public bool IsSubmitted
    {
        get => _isSubmitted;
        set
        {
            if (_isSubmitted == value)
            {
                return;
            }

            _isSubmitted = value;
            IsChanged = true;
        }
    }
    
    private DateTime _submittedDate = SqlDateTime.MinValue.Value;
    public DateTime SubmittedDate
    {
        get => _submittedDate;
        set
        {
            if (_submittedDate == value)
            {
                return;
            }
        
            _submittedDate = value;
            IsChanged = true;
        }
    }
    
    private string _agseamwoid = string.Empty;
    public string AGSEAMWOID
    {
        get => _agseamwoid;
        set
        {
            if (_agseamwoid == value)
            {
                return;
            }

            _agseamwoid = value;
            IsChanged = true;
        }
    }
    
    private string _agseamwrid = string.Empty;
    public string AGSEAMWRID
    {
        get => _agseamwrid;
        set
        {
            if (_agseamwrid == value)
            {
                return;
            }

            _agseamwrid = value;
            IsChanged = true;
        }
    }
    
    private string _agseamentityid = string.Empty;
    public string AGSEAMEntityID
    {
        get => _agseamentityid;
        set
        {
            if (_agseamentityid == value)
            {
                return;
            }

            _agseamentityid = value;
            IsChanged = true;
        }
    }
    
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
            {
                return;
            }

            _name = value;
            IsChanged = true;
        }
    }
    
    private string _headerTitle = string.Empty;
    public string HeaderTitle
    {
        get => _headerTitle;
        set
        {
            if (_headerTitle == value)
            {
                return;
            }

            _headerTitle = value;
            IsChanged = true;
        }
    }
    
    private string _agseampriorityid = string.Empty;
    public string AGSEAMPriorityID
    {
        get => _agseampriorityid;
        set
        {
            if (_agseampriorityid == value)
            {
                return;
            }

            _agseampriorityid = value;
            IsChanged = true;
        }
    }
    
    private string _agseamwotype = string.Empty;
    public string AGSEAMWOTYPE
    {
        get => _agseamwotype;
        set
        {
            if (_agseamwotype == value)
            {
                return;
            }

            _agseamwotype = value;
            IsChanged = true;
        }
    }
    
    private string _agseamwostatusid = string.Empty;
    public string AGSEAMWOStatusID
    {
        get => _agseamwostatusid;
        set
        {
            if (_agseamwostatusid == value)
            {
                return;
            }

            _agseamwostatusid = value;
            IsChanged = true;
        }
    }
    
    private DateTime _agseamplanningstartdate = SqlDateTime.MinValue.Value;
    public DateTime AGSEAMPlanningStartDate
    {
        get => _agseamplanningstartdate;
        set
        {
            if (_agseamplanningstartdate == value)
            {
                return;
            }
        
            _agseamplanningstartdate = value;
            IsChanged = true;
        }
    }
    
    private DateTime _agseamplanningenddate = SqlDateTime.MinValue.Value;
    public DateTime AGSEAMPlanningEndDate
    {
        get => _agseamplanningenddate;
        set
        {
            if (_agseamplanningenddate == value)
            {
                return;
            }
        
            _agseamplanningenddate = value;
            IsChanged = true;
        }
    }
    
    private NoYes _entityShutDown = NoYes.None;
    public NoYes EntityShutDown
    {
        get => _entityShutDown;
        set
        {
            if (_entityShutDown == value)
            {
                return;
            }

            _entityShutDown = value;
            IsChanged = true;
        }
    }
    
    private DateTime _woCloseDate = SqlDateTime.MinValue.Value;
    public DateTime WOCloseDate
    {
        get => _woCloseDate;
        set
        {
            if (_woCloseDate == value)
            {
                return;
            }
        
            _woCloseDate = value;
            IsChanged = true;
        }
    }
    
    private NoYes _agseamsuspend = NoYes.None;
    public NoYes AGSEAMSuspend
    {
        get => _agseamsuspend;
        set
        {
            if (_agseamsuspend == value)
            {
                return;
            }

            _agseamsuspend = value;
            IsChanged = true;
        }
    }
    
    private string _notes = string.Empty;
    public string Notes
    {
        get => _notes;
        set
        {
            if (_notes == value)
            {
                return;
            }

            _notes = value;
            IsChanged = true;
        }
    }

    public ICollection<WorkOrderLine> WorkOrderLines { get; set; } = new List<WorkOrderLine>();
    
    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(WorkOrderHeaderId)] = _workOrderHeaderId;
        OriginalValues[nameof(IsSubmitted)] = _isSubmitted;
        OriginalValues[nameof(SubmittedDate)] = _submittedDate;
        OriginalValues[nameof(AGSEAMWOID)] = _agseamwoid;
        OriginalValues[nameof(AGSEAMWRID)] = _agseamwrid;
        OriginalValues[nameof(AGSEAMEntityID)] = _agseamentityid;
        OriginalValues[nameof(Name)] = _name;
        OriginalValues[nameof(HeaderTitle)] = _headerTitle;
        OriginalValues[nameof(AGSEAMPriorityID)] = _agseampriorityid;
        OriginalValues[nameof(AGSEAMWOTYPE)] = _agseamwotype;
        OriginalValues[nameof(AGSEAMWOStatusID)] = _agseamwostatusid;
        OriginalValues[nameof(AGSEAMPlanningStartDate)] = _agseamplanningstartdate;
        OriginalValues[nameof(AGSEAMPlanningEndDate)] = _agseamplanningenddate;
        OriginalValues[nameof(EntityShutDown)] = _entityShutDown;
        OriginalValues[nameof(WOCloseDate)] = _woCloseDate;
        OriginalValues[nameof(AGSEAMSuspend)] = _agseamsuspend;
        OriginalValues[nameof(Notes)] = _notes;
        base.AcceptChanges();
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        _workOrderHeaderId = OriginalValues[nameof(WorkOrderHeaderId)] as int? ?? 0;
        _isSubmitted = OriginalValues[nameof(IsSubmitted)] as bool? ?? false;
        _submittedDate = OriginalValues[nameof(SubmittedDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _agseamwoid = OriginalValues[nameof(AGSEAMWOID)] as string ?? "";
        _agseamwrid = OriginalValues[nameof(AGSEAMWRID)] as string ?? "";
        _agseamentityid = OriginalValues[nameof(AGSEAMEntityID)] as string ?? "";
        _name = OriginalValues[nameof(Name)] as string ?? "";
        _headerTitle = OriginalValues[nameof(HeaderTitle)] as string ?? "";
        _agseampriorityid = OriginalValues[nameof(AGSEAMPriorityID)] as string ?? "";
        _agseamwotype = OriginalValues[nameof(AGSEAMWOTYPE)] as string ?? "";
        _agseamwostatusid = OriginalValues[nameof(AGSEAMWOStatusID)] as string ?? "";
        _agseamplanningstartdate = OriginalValues[nameof(AGSEAMPlanningStartDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _agseamplanningenddate = OriginalValues[nameof(AGSEAMPlanningEndDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _entityShutDown = OriginalValues[nameof(EntityShutDown)] as NoYes? ?? NoYes.None;
        _woCloseDate = OriginalValues[nameof(WOCloseDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _agseamsuspend = OriginalValues[nameof(AGSEAMSuspend)] as NoYes? ?? NoYes.None;
        _notes = OriginalValues[nameof(Notes)] as string ?? "";
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not WorkOrderHeader value)
        {
            return;
        }
        
        IsSubmitted = value.IsSubmitted;
        SubmittedDate = value.SubmittedDate;
        AGSEAMWOID = value.AGSEAMWOID;
        AGSEAMWRID = value.AGSEAMWRID;
        AGSEAMEntityID = value.AGSEAMEntityID;
        Name = value.Name;
        HeaderTitle = value.HeaderTitle;
        AGSEAMPriorityID = value.AGSEAMPriorityID;
        AGSEAMWOTYPE = value.AGSEAMWOTYPE;
        AGSEAMWOStatusID = value.AGSEAMWOStatusID;
        AGSEAMPlanningStartDate = value.AGSEAMPlanningStartDate;
        AGSEAMPlanningEndDate = value.AGSEAMPlanningEndDate;
        EntityShutDown = value.EntityShutDown;
        WOCloseDate = value.WOCloseDate;
        AGSEAMSuspend = value.AGSEAMSuspend;
        Notes = value.Notes;
    }
}