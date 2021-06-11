using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class OutputParametersDataModel
    {
        [JsonProperty("IsNullable")] public bool IsNullable { get; set; }
        [JsonProperty("StoredProcedureObjectId")] public int InternalId { get; set; }
        [JsonProperty("StoredProcedureName")] public string Name { get; set; }
        [JsonProperty("ParameterName")] public string ParameterName { get; set; }
        [JsonProperty("SystemTypeName")] public string TypeName { get; set; }
        [JsonProperty("MaxLength")] public int MaxLength { get; set; }
    }
}