using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO
{
    public class NumberSequenceDto
    {
        public string Description { get; init; } = "";
        public string Format { get; init; } = "";
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        public int LastNumber { get; init; }
        public string Name { get; init; } = "";
        public string Module { get; init; } = "";
        public int NumberSequenceId { get; init; }
        public string CreatedBy { get; init; } = "";
        public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
        public string ModifiedBy { get; init; } = "";
        public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    }
}
