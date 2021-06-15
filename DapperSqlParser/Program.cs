using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString = "Server= .\\SQLExpress;Database=ShopParserDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string NameSpaceName = "ShopParserApi.Services.GeneratedClientFile";

        private static async Task Main()
        {



            var spService = new StoredProcedureService(ConnectionString);

            var paramsList = await spService.GenerateModelsListAsync();
            var spNamespace = await CreateSpClient(paramsList.ToArray(), NameSpaceName);
            await WriteGeneratedNamespaceToClientFile(spNamespace);
        }

        public static async Task<string> CreateSpDataModelForOutputParams(StoredProcedureParameters parameters)
        {
            if (parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Output \n\t{{");

            if (parameters.OutputParametersDataModels.First().ParameterName.Contains("JSON_"))//SP returns json
            {
                if (Guid.TryParse(parameters.OutputParametersDataModels.First().ParameterName.Replace("JSON_", ""), out _))
                {
                    //Schema parsing
                    var schemaJson =
                        "{\n  \"$schema\": \"http://json-schema.org/draft-04/schema#\",\n  \"title\": \"ProductJson\",\n  \"type\": \"object\",\n  \"additionalProperties\": false,\n  \"properties\": {\n    \"nameForCatalog\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"id\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"urlForProductCatalog\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"priceCurrency\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"keywords\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"descriptionPlain\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"discountedPrice\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"priceUSD\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"sku\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"price\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"hasDiscount\": {\n      \"type\": \"boolean\"\n    },\n    \"discountPercent\": {\n      \"type\": [\n        \"integer\",\n        \"null\"\n      ],\n      \"format\": \"int32\"\n    },\n    \"Presence\": {\n      \"oneOf\": [\n        {\n          \"type\": \"null\"\n        },\n        {\n          \"$ref\": \"#/definitions/PresenceData\"\n        }\n      ]\n    },\n    \"OptPrice\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"ImageUrls\": {\n      \"type\": [\n        \"array\",\n        \"null\"\n      ],\n      \"items\": {\n        \"type\": \"string\"\n      }\n    },\n    \"SyncDate\": {\n      \"type\": \"string\",\n      \"format\": \"date-time\"\n    },\n    \"ExpirationDate\": {\n      \"type\": \"string\",\n      \"format\": \"date-time\"\n    },\n    \"JsonCategory\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"StringCategory\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"JsonCategorySchema\": {\n      \"type\": [\n        \"null\",\n        \"string\"\n      ]\n    },\n    \"ProductAttribute\": {\n      \"type\": [\n        \"array\",\n        \"null\"\n      ],\n      \"items\": {\n        \"$ref\": \"#/definitions/ProductAttribute\"\n      }\n    },\n    \"ProductPaymentOptions\": {\n      \"type\": [\n        \"array\",\n        \"null\"\n      ],\n      \"items\": {\n        \"$ref\": \"#/definitions/ProductPaymentOption\"\n      }\n    },\n    \"ProductDeliveryOptions\": {\n      \"type\": [\n        \"array\",\n        \"null\"\n      ],\n      \"items\": {\n        \"$ref\": \"#/definitions/ProductDeliveryOption\"\n      }\n    }\n  },\n  \"definitions\": {\n    \"PresenceData\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"isPresenceSure\": {\n          \"type\": \"boolean\"\n        },\n        \"isOrderable\": {\n          \"type\": \"boolean\"\n        },\n        \"isAvailable\": {\n          \"type\": \"boolean\"\n        },\n        \"title\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"isEnding\": {\n          \"type\": \"boolean\"\n        },\n        \"isWait\": {\n          \"type\": \"boolean\"\n        }\n      }\n    },\n    \"ProductAttribute\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"ProductId\": {\n          \"type\": [\n            \"integer\",\n            \"null\"\n          ],\n          \"format\": \"int32\"\n        },\n        \"id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"name\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"group\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"AttributeValues\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Product\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/ProductData\"\n            }\n          ]\n        }\n      }\n    },\n    \"ProductData\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"CompanyId\": {\n          \"type\": [\n            \"integer\",\n            \"null\"\n          ],\n          \"format\": \"int32\"\n        },\n        \"ExternalId\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Title\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Url\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"SyncDate\": {\n          \"type\": \"string\",\n          \"format\": \"date-time\"\n        },\n        \"ExpirationDate\": {\n          \"type\": \"string\",\n          \"format\": \"date-time\"\n        },\n        \"ProductState\": {\n          \"$ref\": \"#/definitions/ProductState\"\n        },\n        \"Description\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Price\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"KeyWords\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"JsonData\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"JsonDataSchema\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Company\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/CompanyData\"\n            }\n          ]\n        },\n        \"Presence\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/PresenceData\"\n            }\n          ]\n        },\n        \"Categories\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/CategoryData\"\n          }\n        },\n        \"ProductPaymentOptions\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/ProductPaymentOption\"\n          }\n        },\n        \"ProductDeliveryOptions\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/ProductDeliveryOption\"\n          }\n        },\n        \"ProductAttribute\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/ProductAttribute\"\n          }\n        }\n      }\n    },\n    \"ProductState\": {\n      \"type\": \"integer\",\n      \"description\": \"\",\n      \"x-enumNames\": [\n        \"Idle\",\n        \"Success\",\n        \"Failed\"\n      ],\n      \"enum\": [\n        0,\n        1,\n        2\n      ]\n    },\n    \"CompanyData\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"SourceId\": {\n          \"type\": [\n            \"integer\",\n            \"null\"\n          ],\n          \"format\": \"int32\"\n        },\n        \"ExternalId\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Name\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Url\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"SyncDate\": {\n          \"type\": \"string\",\n          \"format\": \"date-time\"\n        },\n        \"CompanyState\": {\n          \"$ref\": \"#/definitions/CompanyState\"\n        },\n        \"JsonData\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"JsonDataSchema\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Products\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/ProductData\"\n          }\n        },\n        \"Source\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/CompanySource\"\n            }\n          ]\n        }\n      }\n    },\n    \"CompanyState\": {\n      \"type\": \"integer\",\n      \"description\": \"\",\n      \"x-enumNames\": [\n        \"Idle\",\n        \"Processing\",\n        \"Success\",\n        \"Failed\"\n      ],\n      \"enum\": [\n        0,\n        1,\n        2,\n        3\n      ]\n    },\n    \"CompanySource\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"Name\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Companies\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/CompanyData\"\n          }\n        }\n      }\n    },\n    \"CategoryData\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"caption\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"url\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Products\": {\n          \"type\": [\n            \"array\",\n            \"null\"\n          ],\n          \"items\": {\n            \"$ref\": \"#/definitions/ProductData\"\n          }\n        },\n        \"SupCategoryData\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/CategoryData\"\n            }\n          ]\n        }\n      }\n    },\n    \"ProductPaymentOption\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"ProductId\": {\n          \"type\": [\n            \"integer\",\n            \"null\"\n          ],\n          \"format\": \"int32\"\n        },\n        \"id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"name\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"comment\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Product\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/ProductData\"\n            }\n          ]\n        }\n      }\n    },\n    \"ProductDeliveryOption\": {\n      \"type\": \"object\",\n      \"additionalProperties\": false,\n      \"properties\": {\n        \"Id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"ProductId\": {\n          \"type\": [\n            \"integer\",\n            \"null\"\n          ],\n          \"format\": \"int32\"\n        },\n        \"id\": {\n          \"type\": \"integer\",\n          \"format\": \"int32\"\n        },\n        \"name\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"comment\": {\n          \"type\": [\n            \"null\",\n            \"string\"\n          ]\n        },\n        \"Product\": {\n          \"oneOf\": [\n            {\n              \"type\": \"null\"\n            },\n            {\n              \"$ref\": \"#/definitions/ProductData\"\n            }\n          ]\n        }\n      }\n    }\n  }\n}";
                    var schema = await JsonSchema.FromJsonAsync(schemaJson);
                    var generator = new CSharpGenerator(schema);

                    generator.Settings.Namespace = "MakerNamespace";

                    var generatedClasses = generator.GenerateFile();

                    var nameSpaceBracketIndex = generatedClasses.IndexOf("MakerNamespace", StringComparison.Ordinal) + generator.Settings.Namespace.Length + 2;
                    var resultClasses = generatedClasses.Substring(nameSpaceBracketIndex, generatedClasses.Length - 2 - nameSpaceBracketIndex);
                }
                
            }
            else//Sp returns fields
            {
                foreach (var field in parameters.OutputParametersDataModels.Select(p =>
                        new string(
                            $"\t\t{(p.ParameterName == null ? $"" : $"[Newtonsoft.Json.JsonProperty(\"{p.ParameterName}\")]")} " +//If not nullable -> required
                            $"{(p.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" +//Json field
                            $"public {p.TypeName} " +//Type name
                            $"{(p.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{p.ParameterName.Replace("-", "_")}")} " +//Param name
                            $"{{get; set;}} \n"))//Getter/setter
                ) outputClass.AppendLine(field);
            }
            
            
            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpDataModelForInputParams(StoredProcedureParameters parameters)
        {
            if (parameters.InputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Input \n\t{{");

            foreach (var field in parameters.InputParametersDataModels.Select(p =>
                    new string(
                        $"\t\t{(p.ParameterName == null ? $"" : $"[Newtonsoft.Json.JsonProperty(\"{p.ParameterName.Replace("@", "")}\")]")} " +//If not nullable -> required
                        $"{(p.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" +//Json field
                        $"public {p.TypeName} " +//Type name
                        $"{(p.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{p.ParameterName.Replace("-", "_").Replace("@", "").FirstCharToUpper()}")} " +//Param name
                        $"{{get; set;}} \n"))//Getter/setter
            ) outputClass.AppendLine(field);


            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpClientClass(StoredProcedureParameters parameters)
        {

            var outputClass = new StringBuilder();

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{");//Class name

            if (parameters.InputParametersDataModels != null && parameters.OutputParametersDataModels != null)//IF inputs and outputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input, {parameters.StoredProcedureInfo.Name}Output> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input, {parameters.StoredProcedureInfo.Name}Output> dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>> Execute({parameters.StoredProcedureInfo.Name}Input request)\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\", request);" +
                    $"\n\t\t}}");
            }
            else if (parameters.InputParametersDataModels != null && parameters.OutputParametersDataModels == null)//IF inputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input>dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task Execute({parameters.StoredProcedureInfo.Name}Input request)\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\", request);" +
                    $"\n\t\t}}");
            }
            else//IF outputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<EmptyInputParams, {parameters.StoredProcedureInfo.Name}Output> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<EmptyInputParams, {parameters.StoredProcedureInfo.Name}Output> dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>> Execute()\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\");" +
                    $"\n\t\t}}");
            }


            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpClient(IEnumerable<StoredProcedureParameters> parameters, string namespaceName)
        {

            var outputNamespace = new StringBuilder();
            outputNamespace.AppendLine($"namespace {namespaceName} \n{{");

            foreach (var spParameter in parameters)
            {
                outputNamespace.AppendLine($"\n\t#region {spParameter.StoredProcedureInfo.Name}");//Wrapping every sp into region

                var outputModelClass = await CreateSpDataModelForOutputParams(spParameter);
                var inputModelClass = await CreateSpDataModelForInputParams(spParameter);
                var clientClass = await CreateSpClientClass(spParameter);

                outputNamespace.AppendLine(outputModelClass);
                outputNamespace.AppendLine(inputModelClass);
                outputNamespace.AppendLine(clientClass);

                outputNamespace.AppendLine("\t#endregion");
            }

            outputNamespace.Append("}");
            return await Task.FromResult(outputNamespace.ToString());
        }
        public static async Task WriteGeneratedNamespaceToClientFile(string namespaceString)
        {
            // This will get the current PROJECT directory
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var filePath = Path.Combine(projectPath ?? throw new InvalidOperationException(), @"GeneratedFile\spClient.cs");

            await File.WriteAllTextAsync(filePath, namespaceString);
        }

    }
}