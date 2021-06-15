using DapperSqlParser.Services;
using Xunit;

namespace DapperSqlParser.Tests
{
    public class StoredProcedureServiceTests
    {
        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int64_IfInputIs_bigint()
        {
            //Arrange
            const string testType = "bigint";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int64", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_binary()
        {
            //Arrange
            const string testType = "binary";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Boolean_IfInputIs_bit()
        {
            //Arrange
            const string testType = "bit";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Boolean", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Char_IfInputIs_char()
        {
            //Arrange
            const string testType = "char";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Char", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_date()
        {
            //Arrange
            const string testType = "date";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_datetime()
        {
            //Arrange
            const string testType = "datetime";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_datetime2()
        {
            //Arrange
            const string testType = "datetime2";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTimeOffset_IfInputIs_datetimeoffset()
        {
            //Arrange
            const string testType = "datetimeoffset";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTimeOffset", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_decimal()
        {
            //Arrange
            const string testType = "decimal";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_filestream()
        {
            //Arrange
            const string testType = "filestream";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Double_IfInputIs_float()
        {
            //Arrange
            const string testType = "float";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Double", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlGeography_IfInputIs_geography()
        {
            //Arrange
            const string testType = "geography";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlGeography", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlGeometry_IfInputIs_geometry()
        {
            //Arrange
            const string testType = "geometry";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlGeometry", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlHierarchyId_IfInputIs_hierarchyid()
        {
            //Arrange
            const string testType = "hierarchyid";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlHierarchyId", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_image()
        {
            //Arrange
            const string testType = "image";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int32_IfInputIs_int()
        {
            //Arrange
            const string testType = "int";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int32", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_money()
        {
            //Arrange
            const string testType = "money";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_nchar()
        {
            //Arrange
            const string testType = "nchar";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_ntext()
        {
            //Arrange
            const string testType = "ntext";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_numeric()
        {
            //Arrange
            const string testType = "numeric";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_nvarchar()
        {
            //Arrange
            const string testType = "nvarchar";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Single_IfInputIs_real()
        {
            //Arrange
            const string testType = "real";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Single", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_rowversion()
        {
            //Arrange
            const string testType = "rowversion";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_smalldatetime()
        {
            //Arrange
            const string testType = "smalldatetime";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int16_IfInputIs_smallint()
        {
            //Arrange
            const string testType = "smallint";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int16", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_smallmoney()
        {
            //Arrange
            const string testType = "smallmoney";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Object_IfInputIs_sql_variant()
        {
            //Arrange
            const string testType = "sql_variant";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Object", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_text()
        {
            //Arrange
            const string testType = "text";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_TimeSpan_IfInputIs_time()
        {
            //Arrange
            const string testType = "time";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.TimeSpan", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_timestamp()
        {
            //Arrange
            const string testType = "timestamp";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Byte_IfInputIs_tinyint()
        {
            //Arrange
            const string testType = "tinyint";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Guid_IfInputIs_uniqueidentifier()
        {
            //Arrange
            const string testType = "uniqueidentifier";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Guid", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_varbinary()
        {
            //Arrange
            const string testType = "varbinary";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_varchar()
        {
            //Arrange
            const string testType = "varchar";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_xml()
        {
            //Arrange
            const string testType = "xml";
            var service = new StoredProcedureService(null);

            //Act
            var cSharpType = service.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }


    }
}
