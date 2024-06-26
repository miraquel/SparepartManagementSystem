using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class VersionTracker : BaseModel
{
    private int _versionTrackerId;
    public int VersionTrackerId
    {
        get => _versionTrackerId;
        set
        {
            if (_versionTrackerId == value)
            {
                return;
            }

            _versionTrackerId = value;
            IsChanged = true;
        }
    }

    private string _version = string.Empty;
    public string Version
    {
        get => _version;
        set
        {
            if (_version == value)
            {
                return;
            }

            _version = value;
            IsChanged = true;
        }
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set
        {
            if (_description == value)
            {
                return;
            }

            _description = value;
            IsChanged = true;
        }
    }
    
    private string _physicalLocation = string.Empty;
    public string PhysicalLocation
    {
        get => _physicalLocation;
        set
        {
            if (_physicalLocation == value)
            {
                return;
            }

            _physicalLocation = value;
            IsChanged = true;
        }
    }
    
    private DateTime _publishedDateTime = SqlDateTime.MinValue.Value;
    public DateTime PublishedDateTime
    {
        get => _publishedDateTime;
        set
        {
            if (_publishedDateTime == value)
            {
                return;
            }

            _publishedDateTime = value;
            IsChanged = true;
        }
    }

    private string _sha1Checksum = string.Empty;
    public string Sha1Checksum
    {
        get => _sha1Checksum;
        set
        {
            if (_sha1Checksum == value)
            {
                return;
            }

            _sha1Checksum = value;
            IsChanged = true;
        }
    }
    
    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(VersionTrackerId)] = _versionTrackerId;
        OriginalValues[nameof(Version)] = _version;
        OriginalValues[nameof(Description)] = _description;
        OriginalValues[nameof(PhysicalLocation)] = _physicalLocation;
        OriginalValues[nameof(PublishedDateTime)] = _publishedDateTime;
        OriginalValues[nameof(Sha1Checksum)] = _sha1Checksum;
        base.AcceptChanges();
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }

        _versionTrackerId = OriginalValues[nameof(VersionTrackerId)] as int? ?? 0;
        _version = OriginalValues[nameof(Version)] as string ?? string.Empty;
        _description = OriginalValues[nameof(Description)] as string ?? string.Empty;
        _physicalLocation = OriginalValues[nameof(PhysicalLocation)] as string ?? string.Empty;
        _publishedDateTime = OriginalValues[nameof(PublishedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _sha1Checksum = OriginalValues[nameof(Sha1Checksum)] as string ?? string.Empty;
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not VersionTracker versionTracker)
        {
            return;
        }
        
        Version = versionTracker.Version;
        Description = versionTracker.Description;
        PhysicalLocation = versionTracker.PhysicalLocation;
        PublishedDateTime = versionTracker.PublishedDateTime;
        Sha1Checksum = versionTracker.Sha1Checksum;
    }
}