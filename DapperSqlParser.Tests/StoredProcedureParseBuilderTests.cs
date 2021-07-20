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
        public void AppendStoredProcedureRegionStart_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            const string regionName = "TestRegion";

            const string expected = "\n\t#region TestRegion\r\n";

            //Act
            storedProcedureParseBuilder.AppendStoredProcedureRegionStart(regionName);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendStoredProcedureRegionStart_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            const string regionName = "TestRegion";
            
            // Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureRegionStart(regionName));

        }

        [Fact]
        public void AppendStoredProcedureRegionStart_ThrowsOnRegionNameNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            const string regionName = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureRegionStart(regionName));
        }

        [Fact]
        public void AppendStoredProcedureRegionEnd_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            const string expected = "\t#endregion\r\n";

            //Act
            storedProcedureParseBuilder.AppendStoredProcedureRegionEnd();

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendStoredProcedureRegionEnd_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureRegionEnd());
        }

        [Fact]
        public void AppendStoredProcedureCantParseMessage_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = new StoredProcedureInfo()
            {
                Error = "Error code",
                Name = "TestCase",
                Id = 0
            };

            const string expected = "//Couldn't parse Stored procedure  with name: TestCase because of internal error: Error code\n\t\r\n";

            //Act
            storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(storedProcedureInfo);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendStoredProcedureCantParseMessage_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder =null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = new StoredProcedureInfo()
            {
                Error = "Error code",
                Name = "TestCase",
                Id = 0
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(storedProcedureInfo));
        }

        [Fact]
        public void AppendStoredProcedureCantParseMessage_ThrowsOnStoredProcedureInfoNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(storedProcedureInfo));
        }

        [Fact]
        public void AppendStoredProcedureNotFoundMessage_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureInfo storedProcedureInfo = new StoredProcedureInfo() { Name = "TestCase" };

            const string expected = "//Model for TestCase was not found, could not parse this Stored Procedure!\r\n";

            //Act
            storedProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(storedProcedureInfo);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendStoredProcedureNotFoundMessage_ThrowsOnStoredProcedureNameNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            const StoredProcedureInfo storedProcedureName = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(storedProcedureName));
        }

        [Fact]
        public void AppendClientConstructor_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            const string expected = "\t\tpublic TestCase(IDapperExecutor<TestCaseInput, TestCaseOutput> dapperExecutor)\n" +
                                    "\t\t{\n\t" +
                                    "\t\tthis._dapperExecutor = dapperExecutor;\n" +
                                    "\t\t}\r\n";

            //Act
            storedProcedureParseBuilder.AppendClientConstructor(storedProcedureParameters);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendClientConstructor_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendClientConstructor(storedProcedureParameters));
        }

        [Fact]
        public void AppendClientConstructor_ThrowsOnInputParametersDataModelsOrOutputParametersDataModelsNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = new StoredProcedureParameters()
            {
                OutputParametersDataModels = null,
                InputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase"}
                }
            };
            
            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendClientConstructor(storedProcedureParameters));
        }

        [Fact]
        public void AppendClientConstructor_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(()=> storedProcedureParseBuilder.AppendClientConstructor(storedProcedureParameters));
        }

        [Fact]
        public void AppendExecutorMethodWithoutJson_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            const string expected = "\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCaseOutput>> Execute(TestCaseInput request )\n" +
                                    "\t\t{\n" +
                                    "\t\t\treturn _dapperExecutor.ExecuteAsync(\"TestCase\", request);\n" +
                                    "\t\t}\r\n";

            //Act
            storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters, false);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendExecutorMethodWithoutJson_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters,false));
        }

        [Fact]
        public void AppendExecutorMethodWithoutJson_ThrowsOnInputParametersDataModelsOrOutputParametersDataModelsNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = new StoredProcedureParameters()
            {
                OutputParametersDataModels = null,
                InputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase"}
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters,false));
        }

        [Fact]
        public void AppendExecutorMethodWithoutJson_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters,false));
        }

        [Fact]
        public void AppendExecutorMethodWithJson_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            const string expected = "\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCaseOutput>> Execute(TestCaseInput request )\n" +
                                    "\t\t{\n" +
                                    "\t\t\treturn _dapperExecutor.ExecuteJsonAsync(\"TestCase\", request);\n" +
                                    "\t\t}\r\n";

            //Act
            storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters, true);
            
            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendExecutorMethodWithJson_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters, true));
        }

        [Fact]
        public void AppendExecutorMethodWithJson_ThrowsOnInputParametersDataModelsOrOutputParametersDataModelsNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = new StoredProcedureParameters()
            {
                OutputParametersDataModels = null,
                InputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase"}
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters, true));
        }

        [Fact]
        public void AppendExecutorMethodWithJson_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendExecutorMethod(storedProcedureParameters, true));
        }

        [Fact]
        public void AppendIDapperExecutorField_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            const string expected = "\t\tprivate readonly IDapperExecutor<TestCaseInput, TestCaseOutput> _dapperExecutor;\r\n";

            //Act
            storedProcedureParseBuilder.AppendIDapperExecutorField(storedProcedureParameters);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public void AppendIDapperExecutorField_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendIDapperExecutorField(storedProcedureParameters));
        }

        [Fact]
        public void AppendIDapperExecutorField_ThrowsOnInputParametersDataModelsOrOutputParametersDataModelsNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = new StoredProcedureParameters()
            {
                OutputParametersDataModels = null,
                InputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase"}
                }
            };

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendIDapperExecutorField(storedProcedureParameters));
        }

        [Fact]
        public void AppendIDapperExecutorField_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() => storedProcedureParseBuilder.AppendIDapperExecutorField(storedProcedureParameters));
        }

        [Fact]
        public async void AppendExtractedCsSharpCode_WorksCorrectly()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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

            const string expected = "\tpublic class TestCaseOutput \n" +
                                    "\t{\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\n" +
                                    "\t\tpublic System.String Test0 {get; set;} \n\r\n\t}\n\r\n\tpublic class TestCaseInput \n" +
                                    "\t{\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test0\")] [System.ComponentModel.DataAnnotations.Required()] public System.String Test0 {get; set;} \n\r\n" +
                                    "\t}\n\r\n\tpublic class TestCase \n" +
                                    "\t{\r\n" +
                                    "\t\tprivate readonly IDapperExecutor<TestCaseInput, TestCaseOutput> _dapperExecutor;\r\n" +
                                    "\t\tpublic TestCase(IDapperExecutor<TestCaseInput, TestCaseOutput> dapperExecutor)\n" +
                                    "\t\t{\n\t\t\tthis._dapperExecutor = dapperExecutor;\n" +
                                    "\t\t}\r\n" +
                                    "\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TestCaseOutput>> Execute(TestCaseInput request )\n" +
                                    "\t\t{\n" +
                                    "\t\t\treturn _dapperExecutor.ExecuteAsync(\"TestCase\", request);\n" +
                                    "\t\t}\r\n" +
                                    "\t}\n";

            //Act
            await storedProcedureParseBuilder.AppendExtractedCsSharpCode(storedProcedureParameters);

            //Assert
            Assert.Equal(expected, stringBuilder.ToString());
        }

        [Fact]
        public async void AppendExtractedCsSharpCode_ThrowsOnStoredProcedureParametersNull()
        {
            //Arrange
            StringBuilder stringBuilder = new StringBuilder();

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

            StoredProcedureParameters storedProcedureParameters = null;

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>( async () => await storedProcedureParseBuilder.AppendExtractedCsSharpCode(storedProcedureParameters));
        }
        
        [Fact]
        public async void AppendExtractedCsSharpCode_ThrowsOnStringBuilderNull()
        {
            //Arrange
            StringBuilder stringBuilder = null;

            StoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder();
            storedProcedureParseBuilder.SetStringBuilder(stringBuilder);

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
                }
            };

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await storedProcedureParseBuilder.AppendExtractedCsSharpCode(storedProcedureParameters));
        }
    }
}