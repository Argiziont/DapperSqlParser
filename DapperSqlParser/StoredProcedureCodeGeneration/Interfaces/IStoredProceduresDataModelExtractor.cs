using System.Threading.Tasks;
using DapperSqlParser.Models;

namespace DapperSqlParser.StoredProcedureCodeGeneration.Interfaces
{
    public interface IStoredProceduresDataModelExtractor
    {
        public StoredProcedureParameters Parameters { get; set; }
        public Task<string> CreateSpDataModelForOutputParams();
        public Task<string> CreateSpDataModelForOutputParamsJson();
        public Task<string> CreateSpDataModelForInputParams();
        public Task<string> CreateSpDataModelForInputParamsJson(int jsonSchemaStartIndex, int jsonSchemaEndIndex);
    }
}