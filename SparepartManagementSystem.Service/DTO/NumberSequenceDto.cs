using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO
{
    public class NumberSequenceDto
    {
        public string Description { get; set; } = "";
        public string Format { get; set; } = "";
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
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
