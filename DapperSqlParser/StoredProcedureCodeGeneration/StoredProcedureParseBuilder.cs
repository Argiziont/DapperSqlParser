using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProcedureParseBuilder: IStoredProcedureParseBuilder
    {
        private StringBuilder _internalStringBuilder;
        public IStoredProceduresDataModelExtractor ModelExtractor { get; }

        public StoredProcedureParseBuilder(IStoredProceduresDataModelExtractor modelExtractor)
        {
            ModelExtractor = modelExtractor;
        }
        public void SetStringBuilder(StringBuilder stringBuilder)
        {
            _internalStringBuilder = stringBuilder;
        }
        
        public async Task AppendExtractedCsSharpCode(StoredProcedureParameters spParameter)
        {
            if (spParameter == null) throw new ArgumentNullException(nameof(spParameter));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            ModelExtractor.Parameters = spParameter;

            string outputModelClass =
                await ModelExtractor.CreateSpDataModelForOutputParams();
            _internalStringBuilder.AppendLine(outputModelClass);

            string inputModelClass =
                await ModelExtractor.CreateSpDataModelForInputParams();
            _internalStringBuilder.AppendLine(inputModelClass);

            AppendStoredProcedureClientClass(spParameter);
        }

        public void AppendStoredProcedureRegionStart(string regionName)
        {
            if (regionName == null) throw new ArgumentNullException(nameof(regionName));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(
                $"\n\t#region {regionName}"); //Wrapping every sp into region
        }

        public void AppendStoredProcedureRegionEnd()
        {
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine("\t#endregion");
        }

        public void AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine("//Couldn't parse Stored procedure  with name: " +
                                           $"{storedProcedureInfo.Name} because of internal error: " +
                                           $"{storedProcedureInfo.Error}\n\t#endregion");
        }

        public void AppendStoredProcedureNotFoundMessage(string storedProcedureName)
        {
            _internalStringBuilder.AppendLine(
                $"//Model for {storedProcedureName} was not found, could not parse this Stored Procedure!");
        }

        public void AppendClientConstructor(StoredProcedureParameters parameters)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));


            _internalStringBuilder.AppendLine(
                $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> dapperExecutor)\n\t\t{{" + //Ctor 
                "\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                "\n\t\t}");
        }

        public void AppendExecutorMethod(StoredProcedureParameters parameters, bool spReturnJsonFlag)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(
                $"\t\tpublic System.Threading.Tasks.Task{(parameters.OutputParametersDataModels != null ? $"<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>>" : "")} " +
                $"Execute({(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input request" : "")} )\n\t\t{{" + //Execute method
                $"\n\t\t\treturn _dapperExecutor.{(spReturnJsonFlag ? "ExecuteJsonAsync" : "ExecuteAsync")}(\"{parameters.StoredProcedureInfo.Name}\"{(parameters.InputParametersDataModels != null ? ", request" : "")});" +
                "\n\t\t}"); //If input and output
        }

        public void AppendIDapperExecutorField(StoredProcedureParameters parameters)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(
                "\t\tprivate readonly " +
                $"IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> _dapperExecutor;");
        }

        private void AppendStoredProcedureClientClass(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            _internalStringBuilder.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{"); //Class name

            AppendIDapperExecutorField(parameters);
            AppendClientConstructor(parameters);
            AppendExecutorMethod(parameters, StoreProcedureInputIsJson(parameters.OutputParametersDataModels?.First().ParameterName));

            _internalStringBuilder.Append("\t}\n");
        }

        private bool StoreProcedureInputIsJson(string inputParameterName)
        {
            return inputParameterName != null && Guid.TryParse(inputParameterName.Replace("JSON_", ""), out _);
        }
    }
}