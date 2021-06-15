using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureText
    {
        [JsonProperty("definition")] public bool Definition { get; set; }
    }
}