using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.JsonNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    //public  class StoredProceduresDataModelExtractor: IStoredProceduresDataModelExtractor
    //{
    //    public StoredProcedureParameters Parameters { get; set; }

    //    public StoredProceduresDataModelExtractor(StoredProcedureParameters parameters)
    //    {
    //        Parameters = parameters;
    //    }
        
    //    public StoredProceduresDataModelExtractor()
    //    {
    //    }
      
       
    //}
}