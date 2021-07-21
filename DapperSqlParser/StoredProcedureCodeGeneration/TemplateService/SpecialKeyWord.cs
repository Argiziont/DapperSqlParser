namespace DapperSqlParser.StoredProcedureCodeGeneration.TemplateService
{
    public static class SpecialKeyWord
    {
        public static string Readonly => "readonly";
        public static string Class => "class";
        public static string PropertyGetSetAccessor => "{get; set;}";
        public static string This => "this";
        public static string Namespace => "namespace";
        public static string Region => "#region";
        public static string Endregion => "#endregion";
    }
}