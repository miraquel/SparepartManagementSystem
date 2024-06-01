using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain
{
    public class NumberSequence : BaseModel
    {
        private int _numberSequenceId;
        public int NumberSequenceId
        {
            get => _numberSequenceId;
            set
            {
                if (_numberSequenceId == value)
                {
                    return;
                }

                _numberSequenceId = value;
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
        
        private string _format = string.Empty;
        public string Format
        {
            get => _format;
            set
            {
                if (_format == value)
                {
                    return;
                }

                _format = value;
                IsChanged = true;
            }
        }
        
        private int _lastNumber;
        public int LastNumber
        {
            get => _lastNumber;
            set
            {
                if (_lastNumber == value)
                {
                    return;
                }

                _lastNumber = value;
                IsChanged = true;
            }
        }
        
        private string _module = string.Empty;
        public string Module
        {
            get => _module;
            set
            {
                if (_module == value)
                {
                    return;
                }

                _module = value;
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
            
            OriginalValues[nameof(NumberSequenceId)] = _numberSequenceId;
            OriginalValues[nameof(Name)] = _name;
            OriginalValues[nameof(Description)] = _description;
            OriginalValues[nameof(Format)] = _format;
            OriginalValues[nameof(LastNumber)] = _lastNumber;
            OriginalValues[nameof(Module)] = _module;
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
            
            _numberSequenceId = OriginalValues[nameof(NumberSequenceId)] as int? ?? 0;
            _name = OriginalValues[nameof(Name)] as string ?? string.Empty;
            _description = OriginalValues[nameof(Description)] as string ?? string.Empty;
            _format = OriginalValues[nameof(Format)] as string ?? string.Empty;
            _lastNumber = OriginalValues[nameof(LastNumber)] as int? ?? 0;
            _module = OriginalValues[nameof(Module)] as string ?? string.Empty;
            _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
            _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
            _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
            _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
            
            IsChanged = false;
        }

        public override void UpdateProperties<T>(T source)
        {
            if (!(source is NumberSequence value))
            {
                return;
            }
            
            NumberSequenceId = value.NumberSequenceId;
            Name = value.Name;
            Description = value.Description;
            Format = value.Format;
            LastNumber = value.LastNumber;
            Module = value.Module;
            CreatedBy = value.CreatedBy;
            CreatedDateTime = value.CreatedDateTime;
            ModifiedBy = value.ModifiedBy;
            ModifiedDateTime = value.ModifiedDateTime;
        }
    }
}