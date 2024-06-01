using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO
{
    public class NumberSequenceDto
    {
        public string Description { get; init; } = string.Empty;
        public string Format { get; init; } = string.Empty;
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        public int LastNumber { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Module { get; init; } = string.Empty;
        public int NumberSequenceId { get; init; }
        public string CreatedBy { get; init; } = string.Empty;
        public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
        public string ModifiedBy { get; init; } = string.Empty;
        public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    }
}
