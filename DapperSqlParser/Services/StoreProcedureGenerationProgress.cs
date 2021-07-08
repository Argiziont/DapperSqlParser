namespace DapperSqlParser.Services
{
    public class StoreProcedureGenerationProgress
    {
        //current progress
        public int CurrentProgressAmount { get; set; }
        //total progress
        public int TotalProgressAmount { get; set; }
        //some message to pass to the UI of current progress
        public string CurrentProgressMessage { get; set; }
    }
}