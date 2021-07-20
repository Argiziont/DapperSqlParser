using System.Threading.Tasks;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface IStoredProcedureCodeGenerator
    {
        public Task<string> GenerateAsync();
    }
}