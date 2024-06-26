using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class WorkOrderLine : BaseModel
{
    private int _workOrderLineId;
    public int WorkOrderLineId
    {
        get => _workOrderLineId;
        set
        {
            if (_workOrderLineId == value)
            {
                return;
            }

            _workOrderLineId = value;
            IsChanged = true;
        }
    }
    
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
    
    private int _line;
    public int Line
    {
        get => _line;
        set
        {
            if (_line == value)
            {
                return;
            }

            _line = value;
            IsChanged = true;
        }
    }
    
    private string _lineTitle = string.Empty;
    public string LineTitle
    {
        get => _lineTitle;
        set
        {
            if (_lineTitle == value)
            {
                return;
            }

            _lineTitle = value;
            IsChanged = true;
        }
    }
    
    private string _entityId = string.Empty;
    public string EntityId
    {
        get => _entityId;
        set
        {
            if (_entityId == value)
            {
                return;
            }

            _entityId = value;
            IsChanged = true;
        }
    }
    
    private NoYes _entityShutdown = NoYes.None;
    public NoYes EntityShutdown
    {
        get => _entityShutdown;
        set
        {
            if (_entityShutdown == value)
            {
                return;
            }

            _entityShutdown = value;
            IsChanged = true;
        }
    }
    
    private string _workOrderType = string.Empty;
    public string WorkOrderType
    {
        get => _workOrderType;
        set
        {
            if (_workOrderType == value)
            {
                return;
            }

            _workOrderType = value;
            IsChanged = true;
        }
    }
    
    private string _taskId = string.Empty;
    public string TaskId
    {
        get => _taskId;
        set
        {
            if (_taskId == value)
            {
                return;
            }

            _taskId = value;
            IsChanged = true;
        }
    }
    
    private string _condition = string.Empty;
    public string Condition
    {
        get => _condition;
        set
        {
            if (_condition == value)
            {
                return;
            }

            _condition = value;
            IsChanged = true;
        }
    }
    
    private DateTime _planningStartDate = SqlDateTime.MinValue.Value;
    public DateTime PlanningStartDate
    {
        get => _planningStartDate;
        set
        {
            if (_planningStartDate == value)
            {
                return;
            }

            _planningStartDate = value;
            IsChanged = true;
        }
    }
    
    private DateTime _planningEndDate = SqlDateTime.MinValue.Value;
    public DateTime PlanningEndDate
    {
        get => _planningEndDate;
        set
        {
            if (_planningEndDate == value)
            {
                return;
            }

            _planningEndDate = value;
            IsChanged = true;
        }
    }
    
    private string _supervisor = string.Empty;
    public string Supervisor
    {
        get => _supervisor;
        set
        {
            if (_supervisor == value)
            {
                return;
            }

            _supervisor = value;
            IsChanged = true;
        }
    }
    
    private string _calendarId = string.Empty;
    public string CalendarId
    {
        get => _calendarId;
        set
        {
            if (_calendarId == value)
            {
                return;
            }

            _calendarId = value;
            IsChanged = true;
        }
    }
    
    private string _workOrderStatus = string.Empty;
    public string WorkOrderStatus
    {
        get => _workOrderStatus;
        set
        {
            if (_workOrderStatus == value)
            {
                return;
            }

            _workOrderStatus = value;
            IsChanged = true;
        }
    }
    
    private NoYes _suspend = NoYes.None;
    public NoYes Suspend
    {
        get => _suspend;
        set
        {
            if (_suspend == value)
            {
                return;
            }

            _suspend = value;
            IsChanged = true;
        }
    }
    
    public override void AcceptChanges()
    {
        if (IsChanged)
        {
            IsChanged = false;
        }
        
        OriginalValues[nameof(WorkOrderLineId)] = _workOrderLineId;
        OriginalValues[nameof(WorkOrderHeaderId)] = _workOrderHeaderId;
        OriginalValues[nameof(Line)] = _line;
        OriginalValues[nameof(LineTitle)] = _lineTitle;
        OriginalValues[nameof(EntityId)] = _entityId;
        OriginalValues[nameof(EntityShutdown)] = _entityShutdown;
        OriginalValues[nameof(WorkOrderType)] = _workOrderType;
        OriginalValues[nameof(TaskId)] = _taskId;
        OriginalValues[nameof(Condition)] = _condition;
        OriginalValues[nameof(PlanningStartDate)] = _planningStartDate;
        OriginalValues[nameof(PlanningEndDate)] = _planningEndDate;
        OriginalValues[nameof(Supervisor)] = _supervisor;
        OriginalValues[nameof(CalendarId)] = _calendarId;
        OriginalValues[nameof(WorkOrderStatus)] = _workOrderStatus;
        OriginalValues[nameof(Suspend)] = _suspend;
        base.AcceptChanges();
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        _workOrderLineId = OriginalValues[nameof(WorkOrderLineId)] as int? ?? 0;
        _workOrderHeaderId = OriginalValues[nameof(WorkOrderHeaderId)] as int? ?? 0;
        _line = OriginalValues[nameof(Line)] as int? ?? 0;
        _lineTitle = OriginalValues[nameof(LineTitle)] as string ?? string.Empty;
        _entityId = OriginalValues[nameof(EntityId)] as string ?? string.Empty;
        _entityShutdown = OriginalValues[nameof(EntityShutdown)] as NoYes? ?? NoYes.None;
        _workOrderType = OriginalValues[nameof(WorkOrderType)] as string ?? string.Empty;
        _taskId = OriginalValues[nameof(TaskId)] as string ?? string.Empty;
        _condition = OriginalValues[nameof(Condition)] as string ?? string.Empty;
        _planningStartDate = OriginalValues[nameof(PlanningStartDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _planningEndDate = OriginalValues[nameof(PlanningEndDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _supervisor = OriginalValues[nameof(Supervisor)] as string ?? string.Empty;
        _calendarId = OriginalValues[nameof(CalendarId)] as string ?? string.Empty;
        _workOrderStatus = OriginalValues[nameof(WorkOrderStatus)] as string ?? string.Empty;
        _suspend = OriginalValues[nameof(Suspend)] as NoYes? ?? NoYes.None;
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not WorkOrderLine workOrderLine)
        {
            return;
        }
        
        WorkOrderHeaderId = workOrderLine.WorkOrderHeaderId;
        Line = workOrderLine.Line;
        LineTitle = workOrderLine.LineTitle;
        EntityId = workOrderLine.EntityId;
        EntityShutdown = workOrderLine.EntityShutdown;
        WorkOrderType = workOrderLine.WorkOrderType;
        TaskId = workOrderLine.TaskId;
        Condition = workOrderLine.Condition;
        PlanningStartDate = workOrderLine.PlanningStartDate;
        PlanningEndDate = workOrderLine.PlanningEndDate;
        Supervisor = workOrderLine.Supervisor;
        CalendarId = workOrderLine.CalendarId;
        WorkOrderStatus = workOrderLine.WorkOrderStatus;
        Suspend = workOrderLine.Suspend;
    }
}