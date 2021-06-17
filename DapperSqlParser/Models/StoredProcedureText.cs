using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureText
    {
        [JsonProperty("Definition")] public string Definition { get; set; }
    }
}