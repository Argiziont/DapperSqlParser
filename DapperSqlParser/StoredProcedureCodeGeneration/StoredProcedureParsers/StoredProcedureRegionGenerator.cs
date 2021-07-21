using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;

namespace DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers
{
    public class StoredProcedureRegionGenerator : ICodeGenerator
    {
        private readonly string _storedProcedureName;
        private readonly string _storedProcedureParsedCode;

        public StoredProcedureRegionGenerator(string storedProcedureName, string storedProcedureParsedCode)
        {
            _storedProcedureName = storedProcedureName;
            _storedProcedureParsedCode = storedProcedureParsedCode;
        }

        public async Task<string> GenerateAsync()
        {
            return await Task.FromResult(CreateStoredProcedureRegion());
        }

        private string CreateStoredProcedureRegion()
        {
            StringBuilder clientClass = new StringBuilder();

            clientClass.AppendLine(
                CodeGeneratorUtils.CreateRegionWithName(_storedProcedureName, _storedProcedureParsedCode));

            return clientClass.ToString();
        }
    }
}