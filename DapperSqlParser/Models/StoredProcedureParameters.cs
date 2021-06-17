using Newtonsoft.Json;

namespace DapperSqlParser.Models
{
    public class StoredProcedureParameters
    {
        [JsonProperty("StoredProcedureText")] public StoredProcedureText[] StoredProcedureTextArray { get; set; }
        [JsonIgnore] public StoredProcedureText StoredProcedureText => StoredProcedureTextArray[0];
        [JsonProperty("StoredProcedureInfo")] public StoredProcedureInfo[] StoredProcedureInfoArray { get; set; }
        [JsonIgnore] public StoredProcedureInfo StoredProcedureInfo => StoredProcedureInfoArray[0];
        [JsonProperty("OutputParameters")] public OutputParametersDataModel[] OutputParametersDataModels { get; set; }
        [JsonProperty("InputParameters")] public InputParametersDataModel[] InputParametersDataModels { get; set; }
    }
}