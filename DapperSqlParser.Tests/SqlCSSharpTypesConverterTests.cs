using DapperSqlParser.StoredProcedureCodeGeneration;
using Xunit;

namespace DapperSqlParser.Tests
{
    public class SqlCsSharpTypesConverterTests
    {
        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int64_IfInputIs_bigint()
        {
            //Arrange
            const string testType = "bigint";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int64", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_binary()
        {
            //Arrange
            const string testType = "binary";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Boolean_IfInputIs_bit()
        {
            //Arrange
            const string testType = "bit";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Boolean", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Char_IfInputIs_char()
        {
            //Arrange
            const string testType = "char";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Char", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_date()
        {
            //Arrange
            const string testType = "date";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_datetime()
        {
            //Arrange
            const string testType = "datetime";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_datetime2()
        {
            //Arrange
            const string testType = "datetime2";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTimeOffset_IfInputIs_datetimeoffset()
        {
            //Arrange
            const string testType = "datetimeoffset";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTimeOffset", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_decimal()
        {
            //Arrange
            const string testType = "decimal";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_filestream()
        {
            //Arrange
            const string testType = "filestream";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Double_IfInputIs_float()
        {
            //Arrange
            const string testType = "float";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Double", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlGeography_IfInputIs_geography()
        {
            //Arrange
            const string testType = "geography";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlGeography", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlGeometry_IfInputIs_geometry()
        {
            //Arrange
            const string testType = "geometry";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlGeometry", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_SqlHierarchyId_IfInputIs_hierarchyid()
        {
            //Arrange
            const string testType = "hierarchyid";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("Microsoft.SqlServer.Types.SqlHierarchyId", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_image()
        {
            //Arrange
            const string testType = "image";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int32_IfInputIs_int()
        {
            //Arrange
            const string testType = "int";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int32", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_money()
        {
            //Arrange
            const string testType = "money";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_nchar()
        {
            //Arrange
            const string testType = "nchar";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_ntext()
        {
            //Arrange
            const string testType = "ntext";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_numeric()
        {
            //Arrange
            const string testType = "numeric";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_nvarchar()
        {
            //Arrange
            const string testType = "nvarchar";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Single_IfInputIs_real()
        {
            //Arrange
            const string testType = "real";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Single", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_rowversion()
        {
            //Arrange
            const string testType = "rowversion";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_DateTime_IfInputIs_smalldatetime()
        {
            //Arrange
            const string testType = "smalldatetime";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.DateTime", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Int16_IfInputIs_smallint()
        {
            //Arrange
            const string testType = "smallint";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Int16", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Decimal_IfInputIs_smallmoney()
        {
            //Arrange
            const string testType = "smallmoney";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Decimal", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Object_IfInputIs_sql_variant()
        {
            //Arrange
            const string testType = "sql_variant";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Object", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_text()
        {
            //Arrange
            const string testType = "text";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_TimeSpan_IfInputIs_time()
        {
            //Arrange
            const string testType = "time";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.TimeSpan", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_timestamp()
        {
            //Arrange
            const string testType = "timestamp";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Byte_IfInputIs_tinyint()
        {
            //Arrange
            const string testType = "tinyint";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_Guid_IfInputIs_uniqueidentifier()
        {
            //Arrange
            const string testType = "uniqueidentifier";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Guid", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_ByteArray_IfInputIs_varbinary()
        {
            //Arrange
            const string testType = "varbinary";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.Byte[]", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_varchar()
        {
            //Arrange
            const string testType = "varchar";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }

        [Fact]
        public void ConvertSqlServerFormatToCSharpReturns_String_IfInputIs_xml()
        {
            //Arrange
            const string testType = "xml";


            //Act
            string cSharpType = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(testType);

            //Assert
            Assert.Equal("System.String", cSharpType);
        }
    }
}