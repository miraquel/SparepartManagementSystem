using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain
{
    public class NumberSequence
    {
        public string Description { get; set; } = "";
        public string Format { get; set; } = "";
        public int LastNumber { get; set; }
        public string Name { get; set; } = "";
        public string Module { get; set; } = "";
        public int NumberSequenceId { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;
        public string ModifiedBy { get; set; } = "";
        public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    }
}