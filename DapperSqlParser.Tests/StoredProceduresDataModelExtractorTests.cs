using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration;
using Xunit;

namespace DapperSqlParser.Tests
{
    public class StoredProceduresDataModelExtractorTests
    {
        [Fact]
        public async void CreateSpDataModelForOutputParams_WorksCorrectly()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                OutputParametersDataModels = new[]
                {
                    new OutputParametersDataModel{Name = "Test0",ParameterName = "Test0", IsNullable = false,MaxLength = 100,TypeName = "System.String",InternalId = 0},
                    new OutputParametersDataModel{Name = "Test1",ParameterName = "Test1", IsNullable = false,MaxLength = 200,TypeName = "System.String",InternalId = 1},
                    new OutputParametersDataModel{Name = "Test2",ParameterName = "Test2", IsNullable = false,MaxLength = 300,TypeName = "System.String",InternalId = 2},
                    new OutputParametersDataModel{Name = "Test3",ParameterName = "Test3", IsNullable = false,MaxLength = 400,TypeName = "System.String",InternalId = 3},
                },
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo(){ Name = "TestCase1"}
                }
            });

            const string expected = "\tpublic class TestCase1Output \n\t" +
                                    "{\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test0 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test1\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test1 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test2\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test2 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test3\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test3 {get; set;} \n\r\n" +
                                    "\t" +
                                    "}\n";

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParams();


            //Assert
            Assert.Equal(expected, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForInputParams_WorksCorrectly()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                InputParametersDataModels = new[]
                {
                    new InputParametersDataModel{Name = "Test0",ParameterName = "Test0", IsNullable = false,MaxLength = 100,TypeName = "System.String",InternalId = 0},
                    new InputParametersDataModel{Name = "Test1",ParameterName = "Test1", IsNullable = true,MaxLength = 200,TypeName = "System.Int64",InternalId = 1},
                    new InputParametersDataModel{Name = "Test2",ParameterName = "Test2", IsNullable = false,MaxLength = 300,TypeName = "System.Byte",InternalId = 2},
                    new InputParametersDataModel{Name = "Test3",ParameterName = "Test3", IsNullable = true,MaxLength = 400,TypeName = "System.String",InternalId = 3},
                },
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo(){ Name = "TestCase2"}
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText(){Definition = "Empty"}
                }
            });

            const string expected = "\tpublic class TestCase2Input \n\t" +
                                    "{\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test0\")] [System.ComponentModel.DataAnnotations.Required()] public System.String Test0 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test1\")] public System.Int64 Test1 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test2\")] [System.ComponentModel.DataAnnotations.Required()] public System.Byte Test2 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test3\")] public System.String Test3 {get; set;} \n\r\n" +
                                    "\t" +
                                    "}\n";

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForInputParams();


            //Assert
            Assert.Equal(expected, generatedCode);
        }
    }
}