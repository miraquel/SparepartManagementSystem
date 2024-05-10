using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain
{
    public class NumberSequence
    {
        public static NumberSequence ForUpdate(NumberSequence oldRecord, NumberSequence newRecord)
        {
            return new NumberSequence
            {
                NumberSequenceId = oldRecord.NumberSequenceId,
                Name = oldRecord.Name != newRecord.Name ? newRecord.Name : "",
                Description = oldRecord.Description != newRecord.Description ? newRecord.Description : "",
                Format = oldRecord.Format != newRecord.Format ? newRecord.Format : "",
                LastNumber = oldRecord.LastNumber != newRecord.LastNumber ? newRecord.LastNumber : 0,
                Module = oldRecord.Module != newRecord.Module ? newRecord.Module : "",
                CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
                CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : SqlDateTime.MinValue.Value,
                ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
                ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : SqlDateTime.MinValue.Value
            };
        }
        
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