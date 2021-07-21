using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface IStoredProcedureParseBuilder : IStoredProcedureStringBuilder
    {
        public Task AppendExtractedCsSharpCode(StoredProcedureParameters storedProcedureParameters);
        public Task AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo);
        public Task AppendStoredProcedureNotFoundMessage(StoredProcedureInfo storedProcedureInfo);
    }

    public interface IStoredProcedureStringBuilder
    {
        public void SetStringBuilder(StringBuilder stringBuilder);
    }
}