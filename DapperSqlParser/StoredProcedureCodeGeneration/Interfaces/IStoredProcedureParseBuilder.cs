using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface IStoredProcedureParseBuilder
    {
        public void SetStringBuilder(StringBuilder stringBuilder);
        public Task AppendExtractedCsSharpCode(StoredProcedureParameters spParameter);
        public void AppendStoredProcedureRegionStart(string regionName);
        public void AppendStoredProcedureRegionEnd();
        public void AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo);
        public void AppendStoredProcedureNotFoundMessage(string storedProcedureName);
        public void AppendClientConstructor(StoredProcedureParameters parameters);
        public void AppendExecutorMethod(StoredProcedureParameters parameters, bool spReturnJsonFlag);
        public void AppendIDapperExecutorField(StoredProcedureParameters parameters);
    }
}