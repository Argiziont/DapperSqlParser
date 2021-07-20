using System.Threading.Tasks;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface ICodeGenerator
    {
        public Task<string> GenerateAsync();
    }
}