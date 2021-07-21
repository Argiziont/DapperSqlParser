using System;
using System.Text;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration;
using Xunit;

namespace DapperSqlParser.Tests
{
    public class StoredProcedureParseBuilderTests
    {
        [Fact]
        public async void AppendStoredProcedureCantParseMessage_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = new StoredProcedureInfo()
            {
                Error = "Error code",
                Name = "TestCase",
                Id = 0
            };

            const string expected = "\r\n" +
                                    "\t#region TestCase\r\n" +
                                    "//Couldn't parse Stored procedure  with className: TestCase because of internal error: Error code\n" +
                                    "\t\r\n" +
                                    "\t#endregion\r\n" +
                                    "\r\n" +
                                    "\r\n";

            //Act
            await storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(storedProcedureInfo);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public async void AppendStoredProcedureCantParseMessage_ThrowsOnStoredProcedureInfoNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = null;

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>await storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(storedProcedureInfo));
        }

        [Fact]
        public async void AppendStoredProcedureNotFoundMessage_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = new StoredProcedureInfo() { Name = "TestCase" };

            const string expected = "\r\n" +
                                    "\t#region TestCase\r\n" +
                                    "//Model for TestCase was not found, could not parse this Stored Procedure!\r\n" +
                                    "\t#endregion\r\n" +
                                    "\r\n" +
                                    "\r\n";

            //Act
            await storedProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(storedProcedureInfo);
            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public async void AppendStoredProcedureNotFoundMessage_ThrowsOnStoredProcedureNameNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder);

            const StoredProcedureInfo storedProcedureName = null;

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await storedProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(storedProcedureName));
        }

        [Fact]
        public async void AppendExtractedCsSharpCode_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = new StoredProcedureParameters()
            {
                OutputParametersDataModels = new[]
                {
                    new OutputParametersDataModel{Name = "Test0",ParameterName = "Test0", IsNullable = false,MaxLength = 100,TypeName = "System.String",InternalId = 0}
                },
                InputParametersDataModels = new[]
                {
                    new InputParametersDataModel{Name = "Test0",ParameterName = "Test0", IsNullable = false,MaxLength = 100,TypeName = "System.String",InternalId = 0}
                },
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase"}
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText(){Definition = "Empty"}
                }
            };

            const string expected = "\r\n" +
                                    "\t#region TestCase\r\n" +
                                    "\tpublic class TestCaseOutput\r\n" +
                                    "\t{\r\n" +
                                    "\t\t [Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\r\n" +
                                    " public System.String Test0 {get; set;}\r\n" +
                                    "\r\n" +
                                    "\r\n" +
                                    "\t}\r\n" +
                                    "\r\n" +
                                    "\tpublic class TestCaseInput\r\n" +
                                    "\t{\r\n" +
                                    "\t\t [Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\r\n public System.String Test0 {get; set;}\r\n" +
                                    "\r\n" +
                                    "\r\n" +
                                    "\t}\r\n" +
                                    "\r\n" +
                                    "\tpublic class TestCase\r\n" +
                                    "\t{\r\n" +
                                    "private readonly IDapperExecutor<TestCaseInput, TestCaseOutput> _dapperExecutor;\r\n" +
                                    "\r\n" +
                                    "\t\tpublic TestCase(IDapperExecutor<TestCaseInput, TestCaseOutput> dapperExecutor){\r\n" +
                                    "\t\t\tthis._dapperExecutor = dapperExecutor;\r\n" +
                                    "\t\t}\r\n" +
                                    "\r\n" +
                                    "\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCaseOutput>>Execute(TestCaseInput request ){\r\n" +
                                    "\t\t\treturn _dapperExecutor.ExecuteAsync(\"TestCase\", request);\r\n" +
                                    "\t\t}\r\n" +
                                    "\r\n" +
                                    "\r\n" +
                                    "\t}\r\n" +
                                    "\r\n" +
                                    "\r\n" +
                                    "\t#endregion\r\n" +
                                    "\r\n" +
                                    "\r\n";

            //Act
            await  storedProcedureParseBuilder.AppendExtractedCsSharpCode(storedProcedureParameters);
          
            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public async void AppendExtractedCsSharpCode_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(stringBuilder); ;

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>( async () => await storedProcedureParseBuilder.AppendExtractedCsSharpCode(storedProcedureParameters));
        }
    }
}