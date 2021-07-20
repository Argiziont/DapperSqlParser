using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProcedureParseBuilder: IStoredProcedureParseBuilder
    {
        private StringBuilder _internalStringBuilder;

        public void SetStringBuilder(StringBuilder stringBuilder)
        {
            _internalStringBuilder = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));
        }
       
        public void AppendStoredProcedureRegionStart(string regionName)
        {
            if (regionName == null) throw new ArgumentNullException(nameof(regionName));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateRegionWithName(regionName)); //Wrapping every sp into region
        }

        public void AppendStoredProcedureRegionEnd()
        {
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateEndRegion());
        }

        public void AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateStoredProcedureErrorComment(storedProcedureInfo.Name, storedProcedureInfo.Error));
        }

        public void AppendStoredProcedureNotFoundMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (storedProcedureInfo.Name == null) throw new ArgumentNullException(nameof(storedProcedureInfo.Name));
            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateStoredProcedureNotFoundComment(storedProcedureInfo.Name));
        }

        public void AppendClientConstructor(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

           
            _internalStringBuilder.AppendLine(
                $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> dapperExecutor)\n\t\t{{" + //Ctor 
                "\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                "\n\t\t}");
        }

        public void AppendExecutorMethod(StoredProcedureParameters parameters, bool spReturnJsonFlag)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(
                $"\t\tpublic System.Threading.Tasks.Task{(parameters.OutputParametersDataModels != null ? $"<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>>" : "")} " +
                $"Execute({(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input request" : "")} )\n\t\t{{" + //Execute method
                $"\n\t\t\treturn _dapperExecutor.{(spReturnJsonFlag ? "ExecuteJsonAsync" : "ExecuteAsync")}(\"{parameters.StoredProcedureInfo.Name}\"{(parameters.InputParametersDataModels != null ? ", request" : "")});" +
                "\n\t\t}"); //If input and output
        }

        public void AppendIDapperExecutorField(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(
                "\t\tprivate readonly " +
                $"IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> _dapperExecutor;");
        }
        
        public async Task AppendExtractedCsSharpCode(StoredProcedureParameters spParameter)
        {
            if (spParameter == null) throw new ArgumentNullException(nameof(spParameter));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            StoredProcedureInputModelGenerator storedProcedureInputModelGenerator =
                new StoredProcedureInputModelGenerator(spParameter.InputParametersDataModels, spParameter.StoredProcedureInfo.Name, spParameter.StoredProcedureText.Definition);
            StoredProcedureOutputModelGenerator storedProcedureOutputModelGenerator =
                new StoredProcedureOutputModelGenerator(spParameter.OutputParametersDataModels, spParameter.StoredProcedureInfo.Name, spParameter.StoredProcedureText.Definition);

            string outputModelClass =
                await storedProcedureOutputModelGenerator.GenerateAsync();
            _internalStringBuilder.AppendLine(outputModelClass);

            string inputModelClass =
                await storedProcedureInputModelGenerator.GenerateAsync();
            _internalStringBuilder.AppendLine(inputModelClass);

            AppendStoredProcedureClientClass(spParameter);
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