using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration;
using System;
using DapperSqlParser.Exceptions;
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
                    new OutputParametersDataModel{Name = "Test1",ParameterName = "Test1", IsNullable = true,MaxLength = 200,TypeName = "System.Int64",InternalId = 1},
                    new OutputParametersDataModel{Name = "Test2",ParameterName = "Test2", IsNullable = false,MaxLength = 300,TypeName = "System.Byte",InternalId = 2},
                    new OutputParametersDataModel{Name = "Test3",ParameterName = "Test3", IsNullable = true,MaxLength = 400,TypeName = "System.String",InternalId = 3},
                },
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase1"}
                }
            });

            const string expected = "\tpublic class TestCase1Output \n\t" +
                                    "{\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test0\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.String Test0 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test1\" , Required = Newtonsoft.Json.Required.DisallowNull)]\n\t\tpublic System.Int64 Test1 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test2\" , Required = Newtonsoft.Json.Required.Default)]\n\t\tpublic System.Byte Test2 {get; set;} \n\r\n" +
                                    "\t\t[Newtonsoft.Json.JsonProperty(\"Test3\" , Required = Newtonsoft.Json.Required.DisallowNull)]\n\t\tpublic System.String Test3 {get; set;} \n\r\n" +
                                    "\t" +
                                    "}\n";

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParams();


            //Assert
            Assert.Equal(expected, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForOutputParams_ReturnsEmptyStringOnOutputParametersDataModelsNull()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                OutputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase1"}
                }
            });

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParams();


            //Assert
            Assert.Equal(string.Empty, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForOutputParams_ThrowsOnParametersNull()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(null);

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>( async ()=> await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParams());
        }

        [Fact]
        public async void CreateSpDataModelForOutputJsonParams_WorksCorrectly()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                OutputParametersDataModels = new[]
                {
                    new OutputParametersDataModel{Name = "Test0",ParameterName = "JSON_"+Guid.NewGuid(), IsNullable = false,TypeName = "System.String",InternalId = 0},
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText{Definition = "DEFINITION TEXT/*\tJSON_OUTPUT_SCHEMA_STARTS\r\n{\r\n  \"$schema\": \"http://json-schema.org/draft-04/schema#\",\r\n  \"title\": \"TestCase1\",\r\n   \"type\": \"object\",\r\n  \"additionalProperties\": false,\r\n  \"properties\": {\r\n    \"Test0\": {\r\n      \"type\": \"integer\",\r\n      \"format\": \"int32\"\r\n    },\r\n    \"Test1\": {\r\n      \"type\": [\r\n        \"null\",\r\n        \"string\"\r\n      ]\r\n    },\r\n    \"Test2\": {\r\n      \"type\": [\r\n        \"null\",\r\n        \"string\"\r\n      ]\r\n    }\r\n  }\r\n}\r\nJSON_OUTPUT_SCHEMA_ENDS*/"}
                },

                #region StoredProcedureTestCaseDefition
                //Text above
                /*
                 DEFINITION TEXT/	
                    JSON_OUTPUT_SCHEMA_STARTS
                    {
                      "$schema": "http://json-schema.org/draft-04/schema#",
                      "title": "sp_GetAllCategoriesOutput",
                       "type": "object",
                      "additionalProperties": false,
                      "properties": {
                        "Test0": {
                          "type": "integer",
                          "format": "int32"
                        },
                        "Test1": {
                          "type": [
                            "null",
                            "string"
                          ]
                        },
                        "Test2": {
                          "type": [
                            "null",
                            "string"
                          ]
                        }
                      }
                    }
                    JSON_OUTPUT_SCHEMA_ENDS/
                */
                #endregion

            });

            const string expected = "    [System.CodeDom.Compiler.GeneratedCode(\"NJsonSchema\", \"10.4.4.0 (Newtonsoft.Json v13.0.0.0)\")]\n" +
                                    "    public partial class TestCase1 \n    " +
                                    "{\n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test0\", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public int Test0 { get; set; }\n    \n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test1\", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public string Test1 { get; set; }\n    \n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test2\", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public string Test2 { get; set; }\n    \n    \n    " +
                                    "}";

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParamsJson();


            //Assert
            Assert.Equal(expected, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForOutputJsonParams_ThrowsOnKeywordsMissing()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                OutputParametersDataModels = new[]
                {
                    new OutputParametersDataModel{Name = "Test0",ParameterName = "JSON_"+Guid.NewGuid(), IsNullable = false,TypeName = "System.String",InternalId = 0},
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText{Definition = "DEFINITION TEXT EMPTY"}
                }
            });

            //Assert & Act
            await Assert.ThrowsAsync<NullModelException>(async () => await storedProceduresDataModelExtractor.CreateSpDataModelForOutputParamsJson());
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
                    new StoredProcedureInfo{ Name = "TestCase2"}
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText{Definition = "Empty"}
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

        [Fact]
        public async void CreateSpDataModelForInputParams_ReturnsEmptyStringOnInputParametersDataModelsNull()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                InputParametersDataModels = null,
                StoredProcedureInfoArray = new[]
                {
                    new StoredProcedureInfo{ Name = "TestCase1"}
                }
            });

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForInputParams();


            //Assert
            Assert.Equal(string.Empty, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForInputParams_ThrowsOnParametersNull()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(null);

            //Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await storedProceduresDataModelExtractor.CreateSpDataModelForInputParams());
        }

        [Fact]
        public async void CreateSpDataModelForInputJsonParams_WorksCorrectly()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                InputParametersDataModels = new[]
                {
                    new InputParametersDataModel{Name = "Test0",ParameterName = "JsonInput", IsNullable = false,TypeName = "System.String"},
                   
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText{Definition = "DEFINITION TEXT/*\tJSON_INPUT_SCHEMA_STARTS\r\n{\r\n  \"$schema\": \"http://json-schema.org/draft-04/schema#\",\r\n  \"title\": \"TestCase2\",\r\n   \"type\": \"object\",\r\n  \"additionalProperties\": false,\r\n  \"properties\": {\r\n    \"Test0\": {\r\n      \"type\": \"integer\",\r\n      \"format\": \"int32\"\r\n    },\r\n    \"Test1\": {\r\n      \"type\": [\r\n        \"null\",\r\n        \"string\"\r\n      ]\r\n    },\r\n    \"Test2\": {\r\n      \"type\": [\r\n        \"null\",\r\n        \"string\"\r\n      ]\r\n    }\r\n  }\r\n}\r\nJSON_INPUT_SCHEMA_ENDS*/"}
                }

                #region StoredProcedureTestCaseDefition
                //Text above
                /*
                 DEFINITION TEXT/	
                    JSON_INPUT_SCHEMA_STARTS
                    {
                      "$schema": "http://json-schema.org/draft-04/schema#",
                      "title": "sp_GetAllCategoriesOutput",
                       "type": "object",
                      "additionalProperties": false,
                      "properties": {
                        "Test0": {
                          "type": "integer",
                          "format": "int32"
                        },
                        "Test1": {
                          "type": [
                            "null",
                            "string"
                          ]
                        },
                        "Test2": {
                          "type": [
                            "null",
                            "string"
                          ]
                        }
                      }
                    }
                    JSON_INPUT_SCHEMA_ENDS/
                */
                #endregion


            });

            const string expected = "\t[JsonWrapper(\"JsonInput\")]\n    [System.CodeDom.Compiler.GeneratedCode(\"NJsonSchema\", \"10.4.4.0 (Newtonsoft.Json v13.0.0.0)\")]\n    " +
                                    "public partial class TestCase2 \n    " +
                                    "{\n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test0\", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public int Test0 { get; set; }\n    \n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test1\", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public string Test1 { get; set; }\n    \n        " +
                                    "[Newtonsoft.Json.JsonProperty(\"Test2\", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n        public string Test2 { get; set; }\n    \n    \n    " +
                                    "}";

            //Act
            string generatedCode = await storedProceduresDataModelExtractor.CreateSpDataModelForInputParamsJson();


            //Assert
            Assert.Equal(expected, generatedCode);
        }

        [Fact]
        public async void CreateSpDataModelForInputJsonParams_ThrowsOnKeywordsMissing()
        {
            //Arrange
            StoredProceduresDataModelExtractor storedProceduresDataModelExtractor = new StoredProceduresDataModelExtractor(new StoredProcedureParameters()
            {
                InputParametersDataModels = new[]
                {
                    new InputParametersDataModel{Name = "Test0",ParameterName = "JSON_"+Guid.NewGuid(), IsNullable = false,TypeName = "System.String",InternalId = 0},
                },
                StoredProcedureTextArray = new[]
                {
                    new StoredProcedureText{Definition = "DEFINITION TEXT EMPTY"}
                }
            });

            //Assert & Act
            await Assert.ThrowsAsync<NullModelException>(async () => await storedProceduresDataModelExtractor.CreateSpDataModelForInputParamsJson());
        }

    }
}