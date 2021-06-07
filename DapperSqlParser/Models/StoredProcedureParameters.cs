using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureParameters
    {
        [JsonProperty("OutputParameters")] public OutputParametersDataModel[] OutputParametersDataModels { get; set; }
        [JsonProperty("InputParameters")]  public InputParametersDataModel[] InputParametersDataModels { get; set; }
    }
}