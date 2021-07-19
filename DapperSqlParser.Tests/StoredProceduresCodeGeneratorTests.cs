using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration;
using System.Collections.Generic;
using Xunit;

namespace DapperSqlParser.Tests
{
    public class StoredProceduresCodeGeneratorTests
    {
        [Fact]
        public async void CreateSpClient_WorksCorrectly()
        {
            //Arrange

            StoredProceduresCodeGenerator storedProceduresCodeGenerator =
                new StoredProceduresCodeGenerator(
                    new StoredProcedureParseBuilder(new StoredProceduresDataModelExtractor()))
                {
                    NameSpaceName = "TestNameSpace",
                    Parameters = new List<StoredProcedureParameters>()
                    {
                        new StoredProcedureParameters()
                        {
                            OutputParametersDataModels =
                                new[]
                                {
                                    new OutputParametersDataModel
                                    {
                                        Name = "Test0",
                                        ParameterName = "Test0",
                                        IsNullable = false,
                                        MaxLength = 100,
                                        TypeName = "System.String",
                                        InternalId = 0
                                    }
                                },
                            InputParametersDataModels = new[]
                            {
                                new InputParametersDataModel
                                {
                                    Name = "Test0",
                                    ParameterName = "Test0",
                                    IsNullable = false,
                                    MaxLength = 100,
                                    TypeName = "System.String",
                                    InternalId = 0
                                }
                            },
                            StoredProcedureInfoArray = new[] {new StoredProcedureInfo {Name = "TestCase0"}},
                            StoredProcedureTextArray = new[] {new StoredProcedureText() {Definition = "Empty"}}
                        },
                        new StoredProcedureParameters()
                        {
                            OutputParametersDataModels =
                                new[]
                                {
                                    new OutputParametersDataModel
                                    {
                                        Name = "Test1",
                                        ParameterName = "Test1",
                                        IsNullable = false,
                                        MaxLength = 100,
                                        TypeName = "System.String",
                                        InternalId = 0
                                    }
                                },
                            InputParametersDataModels = new[]
                            {
                                new InputParametersDataModel
                                {
                                    Name = "Test1",
                                    ParameterName = "Test1",
                                    IsNullable = true,
                                    TypeName = "System.Int64",
                                    InternalId = 0
                                }
                            },
                            StoredProcedureInfoArray = new[] {new StoredProcedureInfo {Name = "TestCase1"}},
                            StoredProcedureTextArray = new[] {new StoredProcedureText() {Definition = "Empty"}}
                        }
                    }
                };

            const string expected = "namespace TestNameSpace \n{\r\n\n\t#region TestCase0\r\n\tpublic class TestCase0Output \n\t{\r\n\t\t[Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test0 {get; set;} \n\r\n\t}\n\r\n\tpublic class TestCase0Input \n\t{\r\n\t\t[Newtonsoft.Json.JsonProperty(\"Test0\")] [System.ComponentModel.DataAnnotations.Required()] public System.String Test0 {get; set;} \n\r\n\t}\n\r\n\tpublic class TestCase0 \n\t{\r\n\t\tprivate readonly IDapperExecutor<TestCase0Input, TestCase0Output> _dapperExecutor;\r\n\t\tpublic TestCase0(IDapperExecutor<TestCase0Input, TestCase0Output> dapperExecutor)\n\t\t{\n\t\t\tthis._dapperExecutor = dapperExecutor;\n\t\t}\r\n\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCase0Output>> Execute(TestCase0Input request )\n\t\t{\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"TestCase0\", request);\n\t\t}\r\n\t}\n\t#endregion\r\n\n\t#region TestCase1\r\n\tpublic class TestCase1Output \n\t{\r\n\t\t[Newtonsoft.Json.JsonProperty(\"Test1\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test1 {get; set;} \n\r\n\t}\n\r\n\tpublic class TestCase1Input \n\t{\r\n\t\t[Newtonsoft.Json.JsonProperty(\"Test1\")] public System.Int64 Test1 {get; set;} \n\r\n\t}\n\r\n\tpublic class TestCase1 \n\t{\r\n\t\tprivate readonly IDapperExecutor<TestCase1Input, TestCase1Output> _dapperExecutor;\r\n\t\tpublic TestCase1(IDapperExecutor<TestCase1Input, TestCase1Output> dapperExecutor)\n\t\t{\n\t\t\tthis._dapperExecutor = dapperExecutor;\n\t\t}\r\n\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCase1Output>> Execute(TestCase1Input request )\n\t\t{\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"TestCase1\", request);\n\t\t}\r\n\t}\n\t#endregion\r\n}";

            #region Expected result (escaped text)

            /*namespace TestNameSpace 
            {

	            #region TestCase0
	            public class TestCase0Output 
	            {
		            [Newtonsoft.Json.JsonProperty("Test0" , Required = Newtonsoft.Json.Required.Default)]
		            public System.String Test0 {get; set;} 

	            }

	            public class TestCase0Input 
	            {
		            [Newtonsoft.Json.JsonProperty("Test0")] [System.ComponentModel.DataAnnotations.Required()] public System.String Test0 {get; set;} 

	            }

	            public class TestCase0 
	            {
		            private readonly IDapperExecutor<TestCase0Input, TestCase0Output> _dapperExecutor;
		            public TestCase0(IDapperExecutor<TestCase0Input, TestCase0Output> dapperExecutor)
		            {
			            this._dapperExecutor = dapperExecutor;
		            }
		            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCase0Output>> Execute(TestCase0Input request )
		            {
			            return _dapperExecutor.ExecuteAsync("TestCase0", request);
		            }
	            }
	            #endregion

	            #region TestCase1
	            public class TestCase1Output 
	            {
		            [Newtonsoft.Json.JsonProperty("Test1" , Required = Newtonsoft.Json.Required.Default)]
		            public System.String Test1 {get; set;} 

	            }

	            public class TestCase1Input 
	            {
		            [Newtonsoft.Json.JsonProperty("Test1")] public System.Int64 Test1 {get; set;} 

	            }

	            public class TestCase1 
	            {
		            private readonly IDapperExecutor<TestCase1Input, TestCase1Output> _dapperExecutor;
		            public TestCase1(IDapperExecutor<TestCase1Input, TestCase1Output> dapperExecutor)
		            {
			            this._dapperExecutor = dapperExecutor;
		            }
		            public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCase1Output>> Execute(TestCase1Input request )
		            {
			            return _dapperExecutor.ExecuteAsync("TestCase1", request);
		            }
	            }
	            #endregion
            }*/

            #endregion

            //Act
            string result = await storedProceduresCodeGenerator.CreateSpClient();

            //Assert
            Assert.Equal(expected, result);
        }
    }
}