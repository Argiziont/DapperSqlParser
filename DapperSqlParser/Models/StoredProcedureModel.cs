using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureModel
    {
        [JsonProperty("StoredProcedureObjectId")] public int InternalId { get; set; }
        [JsonProperty("StoredProcedureName")] public string Name { get; set; }
    }
}