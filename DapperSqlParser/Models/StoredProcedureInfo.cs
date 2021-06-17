using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureInfo
    {
        [JsonProperty("StoredProcedureName")] public string Name { get; set; }

        [JsonProperty("StoredProcedureObjectId")]
        public int Id { get; set; }
    }
}