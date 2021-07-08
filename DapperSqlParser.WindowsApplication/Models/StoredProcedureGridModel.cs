namespace DapperSqlParser.WindowsApplication.Models
{
    public class StoredProcedureGridModel
    {
        public bool IsChecked { get; set; }
        public string Title { get; set; }
        public string InputCount { get; set; }
        public string OutputCount { get; set; }
        public string InputTooltip { get; set; }
        public string OutputTooltip { get; set; }
        public string GeneralDetails { get; set; }
    }
}