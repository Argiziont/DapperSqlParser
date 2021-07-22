using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface IStoredProcedureParseBuilder
    {
        public Task AppendStoredProcedureExtractedCode(StoredProcedureParameters storedProcedureParameters);
        public Task AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo);
        public Task AppendStoredProcedureNotFoundMessage(StoredProcedureInfo storedProcedureInfo);
    }
}