using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DapperSqlParser.Services;
using Newtonsoft.Json;
using SpClient;

namespace ShopParserApi.Services.GeneratedClientFile
{
    #region Sp_CountProductsWithCategory

    public class Sp_CountProductsWithCategoryOutput
    {
        [JsonProperty("Result", Required = Required.DisallowNull)]
        public int Result { get; set; }
    }

    public class Sp_CountProductsWithCategoryInput
    {
        [JsonProperty("categoryId")] public int CategoryId { get; set; }
    }

    public class Sp_CountProductsWithCategory
    {
        private readonly IDapperExecutor<Sp_CountProductsWithCategoryInput, Sp_CountProductsWithCategoryOutput>
            _dapperExecutor;

        public Sp_CountProductsWithCategory(
            IDapperExecutor<Sp_CountProductsWithCategoryInput, Sp_CountProductsWithCategoryOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_CountProductsWithCategoryOutput>> Execute(Sp_CountProductsWithCategoryInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_CountProductsWithCategory", request);
        }
    }

    #endregion

    #region Sp_GetAllCategories

    public class Sp_GetAllCategoriesOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.DisallowNull)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull)]
        public int SupCategoryDataId { get; set; }
    }


    public class Sp_GetAllCategories
    {
        private readonly IDapperExecutor<EmptyInputParams, Sp_GetAllCategoriesOutput> _dapperExecutor;

        public Sp_GetAllCategories(IDapperExecutor<EmptyInputParams, Sp_GetAllCategoriesOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetAllCategoriesOutput>> Execute()
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetAllCategories");
        }
    }

    #endregion

    #region Sp_GetAllProducts

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetAllProductsOutput
    {
        [JsonProperty("Id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("CompanyId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("ExternalId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Always)]
        [Required]
        public DateTimeOffset SyncDate { get; set; }

        [JsonProperty("ExpirationDate", Required = Required.Always)]
        [Required]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Always)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Price { get; set; }

        [JsonProperty("KeyWords", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string KeyWords { get; set; }

        [JsonProperty("JsonData", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string JsonDataSchema { get; set; }
    }

    public class Sp_GetAllProducts
    {
        private readonly IDapperExecutor<EmptyInputParams, Sp_GetAllProductsOutput> _dapperExecutor;

        public Sp_GetAllProducts(IDapperExecutor<EmptyInputParams, Sp_GetAllProductsOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetAllProductsOutput>> Execute()
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetAllProducts");
        }
    }

    #endregion

    #region Sp_GetNestedCategoryByParentId

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetNestedCategoryByParentIdOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public int SupCategoryDataId { get; set; }
    }

    public class Sp_GetNestedCategoryByParentIdInput
    {
        [JsonProperty("categoryId")] public int CategoryId { get; set; }
    }

    public class Sp_GetNestedCategoryByParentId
    {
        private readonly IDapperExecutor<Sp_GetNestedCategoryByParentIdInput, Sp_GetNestedCategoryByParentIdOutput>
            _dapperExecutor;

        public Sp_GetNestedCategoryByParentId(
            IDapperExecutor<Sp_GetNestedCategoryByParentIdInput, Sp_GetNestedCategoryByParentIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetNestedCategoryByParentIdOutput>> Execute(
            Sp_GetNestedCategoryByParentIdInput request)
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetNestedCategoryByParentId", request);
        }
    }

    #endregion

    #region Sp_GetNestedCategoryByParentIdAndCompanyId

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetNestedCategoryByParentIdAndCompanyIdOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public int SupCategoryDataId { get; set; }
    }

    public class Sp_GetNestedCategoryByParentIdAndCompanyIdInput
    {
        [JsonProperty("categoryId")] public int CategoryId { get; set; }

        [JsonProperty("companyId")] public int CompanyId { get; set; }
    }

    public class Sp_GetNestedCategoryByParentIdAndCompanyId
    {
        private readonly
            IDapperExecutor<Sp_GetNestedCategoryByParentIdAndCompanyIdInput,
                Sp_GetNestedCategoryByParentIdAndCompanyIdOutput> _dapperExecutor;

        public Sp_GetNestedCategoryByParentIdAndCompanyId(
            IDapperExecutor<Sp_GetNestedCategoryByParentIdAndCompanyIdInput,
                Sp_GetNestedCategoryByParentIdAndCompanyIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetNestedCategoryByParentIdAndCompanyIdOutput>> Execute(
            Sp_GetNestedCategoryByParentIdAndCompanyIdInput request)
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetNestedCategoryByParentIdAndCompanyId", request);
        }
    }

    #endregion

    #region Sp_GetPagedCategories

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetPagedCategoriesOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public int SupCategoryDataId { get; set; }
    }

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetPagedCategoriesInput
    {
        [JsonProperty("page", Required = Required.Always)]
        public int Page { get; set; }

        [JsonProperty("rowsPerPage", Required = Required.Always)]
        public int RowsPerPage { get; set; }
    }

    public class Sp_GetPagedCategories
    {
        private readonly IDapperExecutor<Sp_GetPagedCategoriesInput, Sp_GetPagedCategoriesOutput> _dapperExecutor;

        public Sp_GetPagedCategories(
            IDapperExecutor<Sp_GetPagedCategoriesInput, Sp_GetPagedCategoriesOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetPagedCategoriesOutput>> Execute(Sp_GetPagedCategoriesInput request)
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetPagedCategories", request);
        }
    }

    #endregion

    #region Sp_GetProductsByCategoryId

    public class Sp_GetProductsByCategoryIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("CompanyId", Required = Required.DisallowNull)]
        public int CompanyId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ExpirationDate", Required = Required.Default)]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }

        [JsonProperty("KeyWords", Required = Required.DisallowNull)]
        public string KeyWords { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }
    }

    public class Sp_GetProductsByCategoryIdInput
    {
        [JsonProperty("categoryId")] public int CategoryId { get; set; }
    }

    public class Sp_GetProductsByCategoryId
    {
        private readonly IDapperExecutor<Sp_GetProductsByCategoryIdInput, Sp_GetProductsByCategoryIdOutput>
            _dapperExecutor;

        public Sp_GetProductsByCategoryId(
            IDapperExecutor<Sp_GetProductsByCategoryIdInput, Sp_GetProductsByCategoryIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetProductsByCategoryIdOutput>> Execute(Sp_GetProductsByCategoryIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetProductsByCategoryId", request);
        }
    }

    #endregion

    #region Sp_CountProductsWithCategoryAndCompany

    public class Sp_CountProductsWithCategoryAndCompanyOutput
    {
        [JsonProperty("Result", Required = Required.DisallowNull)]
        public int Result { get; set; }
    }

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_CountProductsWithCategoryAndCompanyInput
    {
        [JsonProperty("companyId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int CompanyId { get; set; }

        [JsonProperty("categoryId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int CategoryId { get; set; }
    }

    public class Sp_CountProductsWithCategoryAndCompany
    {
        private readonly
            IDapperExecutor<Sp_CountProductsWithCategoryAndCompanyInput, Sp_CountProductsWithCategoryAndCompanyOutput>
            _dapperExecutor;

        public Sp_CountProductsWithCategoryAndCompany(
            IDapperExecutor<Sp_CountProductsWithCategoryAndCompanyInput, Sp_CountProductsWithCategoryAndCompanyOutput>
                dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_CountProductsWithCategoryAndCompanyOutput>> Execute(
            Sp_CountProductsWithCategoryAndCompanyInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_CountProductsWithCategoryAndCompany", request);
        }
    }

    #endregion

    #region Sp_GetCompanyByName

    public class Sp_GetCompanyByNameOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("SourceId", Required = Required.DisallowNull)]
        public int SourceId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Name", Required = Required.DisallowNull)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }

        [JsonProperty("CompanyState", Required = Required.Default)]
        public int CompanyState { get; set; }
    }

    public class Sp_GetCompanyByNameInput
    {
        [JsonProperty("companyName")] public string CompanyName { get; set; }
    }

    public class Sp_GetCompanyByName
    {
        private readonly IDapperExecutor<Sp_GetCompanyByNameInput, Sp_GetCompanyByNameOutput> _dapperExecutor;

        public Sp_GetCompanyByName(IDapperExecutor<Sp_GetCompanyByNameInput, Sp_GetCompanyByNameOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetCompanyByNameOutput>> Execute(Sp_GetCompanyByNameInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetCompanyByName", request);
        }
    }

    #endregion

    #region Sp_GetAllCompanies

    public class Sp_GetAllCompaniesOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("SourceId", Required = Required.DisallowNull)]
        public int SourceId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Name", Required = Required.DisallowNull)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }

        [JsonProperty("CompanyState", Required = Required.Default)]
        public int CompanyState { get; set; }
    }


    public class Sp_GetAllCompanies
    {
        private readonly IDapperExecutor<EmptyInputParams, Sp_GetAllCompaniesOutput> _dapperExecutor;

        public Sp_GetAllCompanies(IDapperExecutor<EmptyInputParams, Sp_GetAllCompaniesOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetAllCompaniesOutput>> Execute()
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetAllCompanies");
        }
    }

    #endregion

    #region Sp_GetCompanyById

    public class Sp_GetCompanyByIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("SourceId", Required = Required.DisallowNull)]
        public int SourceId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Name", Required = Required.DisallowNull)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }

        [JsonProperty("CompanyState", Required = Required.Default)]
        public int CompanyState { get; set; }
    }

    public class Sp_GetCompanyByIdInput
    {
        [JsonProperty("companyId")] public int CompanyId { get; set; }
    }

    public class Sp_GetCompanyById
    {
        private readonly IDapperExecutor<Sp_GetCompanyByIdInput, Sp_GetCompanyByIdOutput> _dapperExecutor;

        public Sp_GetCompanyById(IDapperExecutor<Sp_GetCompanyByIdInput, Sp_GetCompanyByIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetCompanyByIdOutput>> Execute(Sp_GetCompanyByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetCompanyById", request);
        }
    }

    #endregion

    #region Sp_CountProductsWithCompany

    public class Sp_CountProductsWithCompanyOutput
    {
        [JsonProperty("Result", Required = Required.DisallowNull)]
        public int Result { get; set; }
    }

    public class Sp_CountProductsWithCompanyInput
    {
        [JsonProperty("companyId")] public int CompanyId { get; set; }
    }

    public class Sp_CountProductsWithCompany
    {
        private readonly IDapperExecutor<Sp_CountProductsWithCompanyInput, Sp_CountProductsWithCompanyOutput>
            _dapperExecutor;

        public Sp_CountProductsWithCompany(
            IDapperExecutor<Sp_CountProductsWithCompanyInput, Sp_CountProductsWithCompanyOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_CountProductsWithCompanyOutput>> Execute(Sp_CountProductsWithCompanyInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_CountProductsWithCompany", request);
        }
    }

    #endregion

    #region Sp_GetAllProductsByCompanyId

    public class Sp_GetAllProductsByCompanyIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("CompanyId", Required = Required.DisallowNull)]
        public int CompanyId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ExpirationDate", Required = Required.Default)]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }

        [JsonProperty("KeyWords", Required = Required.DisallowNull)]
        public string KeyWords { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }
    }

    public class Sp_GetAllProductsByCompanyIdInput
    {
        [JsonProperty("companyId")] public int CompanyId { get; set; }
    }

    public class Sp_GetAllProductsByCompanyId
    {
        private readonly IDapperExecutor<Sp_GetAllProductsByCompanyIdInput, Sp_GetAllProductsByCompanyIdOutput>
            _dapperExecutor;

        public Sp_GetAllProductsByCompanyId(
            IDapperExecutor<Sp_GetAllProductsByCompanyIdInput, Sp_GetAllProductsByCompanyIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetAllProductsByCompanyIdOutput>> Execute(Sp_GetAllProductsByCompanyIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetAllProductsByCompanyId", request);
        }
    }

    #endregion

    #region Sp_GetProductById

    public class Sp_GetProductByIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("CompanyId", Required = Required.DisallowNull)]
        public int CompanyId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ExpirationDate", Required = Required.Default)]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }

        [JsonProperty("KeyWords", Required = Required.DisallowNull)]
        public string KeyWords { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }
    }

    public class Sp_GetProductByIdInput
    {
        [JsonProperty("productId")] public int ProductId { get; set; }
    }

    public class Sp_GetProductById
    {
        private readonly IDapperExecutor<Sp_GetProductByIdInput, Sp_GetProductByIdOutput> _dapperExecutor;

        public Sp_GetProductById(IDapperExecutor<Sp_GetProductByIdInput, Sp_GetProductByIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetProductByIdOutput>> Execute(Sp_GetProductByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetProductById", request);
        }
    }

    #endregion

    #region Sp_GetCategoriesByProductId

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetCategoriesByProductIdOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public int SupCategoryDataId { get; set; }
    }

    public class Sp_GetCategoriesByProductIdInput
    {
        [JsonProperty("productId")] public int ProductId { get; set; }
    }

    public class Sp_GetCategoriesByProductId
    {
        private readonly IDapperExecutor<Sp_GetCategoriesByProductIdInput, Sp_GetCategoriesByProductIdOutput>
            _dapperExecutor;

        public Sp_GetCategoriesByProductId(
            IDapperExecutor<Sp_GetCategoriesByProductIdInput, Sp_GetCategoriesByProductIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetCategoriesByProductIdOutput>> Execute(Sp_GetCategoriesByProductIdInput request)
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetCategoriesByProductId", request);
        }
    }

    #endregion

    #region Sp_GetCategoryById

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetCategoryByIdOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public int SupCategoryDataId { get; set; }
    }

    public class Sp_GetCategoryByIdInput
    {
        [JsonProperty("categoryId")] public int CategoryId { get; set; }
    }

    public class Sp_GetCategoryById
    {
        private readonly IDapperExecutor<Sp_GetCategoryByIdInput, Sp_GetCategoryByIdOutput> _dapperExecutor;

        public Sp_GetCategoryById(IDapperExecutor<Sp_GetCategoryByIdInput, Sp_GetCategoryByIdOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetCategoryByIdOutput>> Execute(Sp_GetCategoryByIdInput request)
        {
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetCategoryById", request);
        }
    }

    #endregion

    #region Sp_GetSuccessfulProductsByCompanyId

    public class Sp_GetSuccessfulProductsByCompanyIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("CompanyId", Required = Required.DisallowNull)]
        public int CompanyId { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ExpirationDate", Required = Required.Default)]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }

        [JsonProperty("KeyWords", Required = Required.DisallowNull)]
        public string KeyWords { get; set; }

        [JsonProperty("JsonData", Required = Required.DisallowNull)]
        public string JsonData { get; set; }

        [JsonProperty("JsonDataSchema", Required = Required.DisallowNull)]
        public string JsonDataSchema { get; set; }
    }

    public class Sp_GetSuccessfulProductsByCompanyIdInput
    {
        [JsonProperty("companyId")] public int CompanyId { get; set; }
    }

    public class Sp_GetSuccessfulProductsByCompanyId
    {
        private readonly
            IDapperExecutor<Sp_GetSuccessfulProductsByCompanyIdInput, Sp_GetSuccessfulProductsByCompanyIdOutput>
            _dapperExecutor;

        public Sp_GetSuccessfulProductsByCompanyId(
            IDapperExecutor<Sp_GetSuccessfulProductsByCompanyIdInput, Sp_GetSuccessfulProductsByCompanyIdOutput>
                dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetSuccessfulProductsByCompanyIdOutput>> Execute(
            Sp_GetSuccessfulProductsByCompanyIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetSuccessfulProductsByCompanyId", request);
        }
    }

    #endregion

    #region Sp_GetStoredProcedureDefinition

    public class Sp_GetStoredProcedureDefinitionOutput
    {
        [JsonProperty("definition", Required = Required.DisallowNull)]
        public string definition { get; set; }
    }

    public class Sp_GetStoredProcedureDefinitionInput
    {
        [JsonProperty("spName")] public string SpName { get; set; }
    }

    public class Sp_GetStoredProcedureDefinition
    {
        private readonly IDapperExecutor<Sp_GetStoredProcedureDefinitionInput, Sp_GetStoredProcedureDefinitionOutput>
            _dapperExecutor;

        public Sp_GetStoredProcedureDefinition(
            IDapperExecutor<Sp_GetStoredProcedureDefinitionInput, Sp_GetStoredProcedureDefinitionOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetStoredProcedureDefinitionOutput>> Execute(
            Sp_GetStoredProcedureDefinitionInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetStoredProcedureDefinition", request);
        }
    }

    #endregion

    #region Sp_GetStoredProcedureParameters

    public class Sp_GetStoredProcedureParametersOutput
    {
        [JsonProperty("object_id", Required = Required.Default)]
        public int object_id { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull)]
        public string name { get; set; }

        [JsonProperty("parameter_id", Required = Required.Default)]
        public int parameter_id { get; set; }

        [JsonProperty("system_type_id", Required = Required.Default)]
        public byte system_type_id { get; set; }

        [JsonProperty("user_type_id", Required = Required.Default)]
        public int user_type_id { get; set; }

        [JsonProperty("max_length", Required = Required.Default)]
        public short max_length { get; set; }

        [JsonProperty("precision", Required = Required.Default)]
        public byte precision { get; set; }

        [JsonProperty("scale", Required = Required.Default)]
        public byte scale { get; set; }

        [JsonProperty("is_output", Required = Required.Default)]
        public bool is_output { get; set; }

        [JsonProperty("is_cursor_ref", Required = Required.Default)]
        public bool is_cursor_ref { get; set; }

        [JsonProperty("has_default_value", Required = Required.Default)]
        public bool has_default_value { get; set; }

        [JsonProperty("is_xml_document", Required = Required.Default)]
        public bool is_xml_document { get; set; }

        [JsonProperty("default_value", Required = Required.DisallowNull)]
        public object default_value { get; set; }

        [JsonProperty("xml_collection_id", Required = Required.Default)]
        public int xml_collection_id { get; set; }

        [JsonProperty("is_readonly", Required = Required.Default)]
        public bool is_readonly { get; set; }

        [JsonProperty("is_nullable", Required = Required.DisallowNull)]
        public bool is_nullable { get; set; }

        [JsonProperty("encryption_type", Required = Required.DisallowNull)]
        public int encryption_type { get; set; }

        [JsonProperty("encryption_type_desc", Required = Required.DisallowNull)]
        public string encryption_type_desc { get; set; }

        [JsonProperty("encryption_algorithm_name", Required = Required.DisallowNull)]
        public string encryption_algorithm_name { get; set; }

        [JsonProperty("column_encryption_key_id", Required = Required.DisallowNull)]
        public int column_encryption_key_id { get; set; }

        [JsonProperty("column_encryption_key_database_name", Required = Required.DisallowNull)]
        public string column_encryption_key_database_name { get; set; }
    }

    public class Sp_GetStoredProcedureParametersInput
    {
        [JsonProperty("spName")] public string SpName { get; set; }
    }

    public class Sp_GetStoredProcedureParameters
    {
        private readonly IDapperExecutor<Sp_GetStoredProcedureParametersInput, Sp_GetStoredProcedureParametersOutput>
            _dapperExecutor;

        public Sp_GetStoredProcedureParameters(
            IDapperExecutor<Sp_GetStoredProcedureParametersInput, Sp_GetStoredProcedureParametersOutput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetStoredProcedureParametersOutput>> Execute(
            Sp_GetStoredProcedureParametersInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetStoredProcedureParameters", request);
        }
    }

    #endregion

    #region Sp_GetStoredProcedureOutputParameters

    public class Sp_GetStoredProcedureOutputParametersOutput
    {
        [JsonProperty("Sp_GetStoredProcedureOutputParametersResult", Required = Required.DisallowNull)]
        public int Sp_GetStoredProcedureOutputParametersResult { get; set; }

        [JsonProperty("is_hidden", Required = Required.DisallowNull)]
        public bool is_hidden { get; set; }

        [JsonProperty("column_ordinal", Required = Required.DisallowNull)]
        public int column_ordinal { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull)]
        public string name { get; set; }

        [JsonProperty("is_nullable", Required = Required.DisallowNull)]
        public bool is_nullable { get; set; }

        [JsonProperty("system_type_id", Required = Required.DisallowNull)]
        public int system_type_id { get; set; }

        [JsonProperty("system_type_name", Required = Required.DisallowNull)]
        public string system_type_name { get; set; }

        [JsonProperty("max_length", Required = Required.DisallowNull)]
        public short max_length { get; set; }

        [JsonProperty("precision", Required = Required.DisallowNull)]
        public byte precision { get; set; }

        [JsonProperty("scale", Required = Required.DisallowNull)]
        public byte scale { get; set; }

        [JsonProperty("collation_name", Required = Required.DisallowNull)]
        public string collation_name { get; set; }

        [JsonProperty("user_type_id", Required = Required.DisallowNull)]
        public int user_type_id { get; set; }

        [JsonProperty("user_type_database", Required = Required.DisallowNull)]
        public string user_type_database { get; set; }

        [JsonProperty("user_type_schema", Required = Required.DisallowNull)]
        public string user_type_schema { get; set; }

        [JsonProperty("user_type_name", Required = Required.DisallowNull)]
        public string user_type_name { get; set; }

        [JsonProperty("assembly_qualified_type_name", Required = Required.DisallowNull)]
        public string assembly_qualified_type_name { get; set; }

        [JsonProperty("xml_collection_id", Required = Required.DisallowNull)]
        public int xml_collection_id { get; set; }

        [JsonProperty("xml_collection_database", Required = Required.DisallowNull)]
        public string xml_collection_database { get; set; }

        [JsonProperty("xml_collection_schema", Required = Required.DisallowNull)]
        public string xml_collection_schema { get; set; }

        [JsonProperty("xml_collection_name", Required = Required.DisallowNull)]
        public string xml_collection_name { get; set; }

        [JsonProperty("is_xml_document", Required = Required.DisallowNull)]
        public bool is_xml_document { get; set; }

        [JsonProperty("is_case_sensitive", Required = Required.DisallowNull)]
        public bool is_case_sensitive { get; set; }

        [JsonProperty("is_fixed_length_clr_type", Required = Required.DisallowNull)]
        public bool is_fixed_length_clr_type { get; set; }

        [JsonProperty("source_server", Required = Required.DisallowNull)]
        public string source_server { get; set; }

        [JsonProperty("source_database", Required = Required.DisallowNull)]
        public string source_database { get; set; }

        [JsonProperty("source_schema", Required = Required.DisallowNull)]
        public string source_schema { get; set; }

        [JsonProperty("source_table", Required = Required.DisallowNull)]
        public string source_table { get; set; }

        [JsonProperty("source_column", Required = Required.DisallowNull)]
        public string source_column { get; set; }

        [JsonProperty("is_identity_column", Required = Required.DisallowNull)]
        public bool is_identity_column { get; set; }

        [JsonProperty("is_part_of_unique_key", Required = Required.DisallowNull)]
        public bool is_part_of_unique_key { get; set; }

        [JsonProperty("is_updateable", Required = Required.DisallowNull)]
        public bool is_updateable { get; set; }

        [JsonProperty("is_computed_column", Required = Required.DisallowNull)]
        public bool is_computed_column { get; set; }

        [JsonProperty("is_sparse_column_set", Required = Required.DisallowNull)]
        public bool is_sparse_column_set { get; set; }

        [JsonProperty("ordinal_in_order_by_list", Required = Required.DisallowNull)]
        public short ordinal_in_order_by_list { get; set; }

        [JsonProperty("order_by_is_descending", Required = Required.DisallowNull)]
        public bool order_by_is_descending { get; set; }

        [JsonProperty("order_by_list_length", Required = Required.DisallowNull)]
        public short order_by_list_length { get; set; }

        [JsonProperty("error_number", Required = Required.DisallowNull)]
        public int error_number { get; set; }

        [JsonProperty("error_severity", Required = Required.DisallowNull)]
        public int error_severity { get; set; }

        [JsonProperty("error_state", Required = Required.DisallowNull)]
        public int error_state { get; set; }

        [JsonProperty("error_message", Required = Required.DisallowNull)]
        public string error_message { get; set; }

        [JsonProperty("error_type", Required = Required.DisallowNull)]
        public int error_type { get; set; }

        [JsonProperty("error_type_desc", Required = Required.DisallowNull)]
        public string error_type_desc { get; set; }
    }

    public class Sp_GetStoredProcedureOutputParametersInput
    {
        [JsonProperty("spName")] public string SpName { get; set; }
    }

    public class Sp_GetStoredProcedureOutputParameters
    {
        private readonly
            IDapperExecutor<Sp_GetStoredProcedureOutputParametersInput, Sp_GetStoredProcedureOutputParametersOutput>
            _dapperExecutor;

        public Sp_GetStoredProcedureOutputParameters(
            IDapperExecutor<Sp_GetStoredProcedureOutputParametersInput, Sp_GetStoredProcedureOutputParametersOutput>
                dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<Sp_GetStoredProcedureOutputParametersOutput>> Execute(
            Sp_GetStoredProcedureOutputParametersInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_GetStoredProcedureOutputParameters", request);
        }
    }

    #endregion

    #region Sp_GetStoredProcedureJsonData

//Model for Sp_GetStoredProcedureJsonData was not found, could not parse this Stored Procedure!

    #endregion

    #region Sp_GetStoredProcedures

//Model for Sp_GetStoredProcedures was not found, could not parse this Stored Procedure!

    #endregion

    #region P_WrapperV1

    public class P_WrapperV1Output
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.DisallowNull)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SupCategoryDataId", Required = Required.DisallowNull)]
        public int SupCategoryDataId { get; set; }
    }


    public class P_WrapperV1
    {
        private readonly IDapperExecutor<EmptyInputParams, P_WrapperV1Output> _dapperExecutor;

        public P_WrapperV1(IDapperExecutor<EmptyInputParams, P_WrapperV1Output> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task<IEnumerable<P_WrapperV1Output>> Execute()
        {
            return _dapperExecutor.ExecuteAsync("P_WrapperV1");
        }
    }

    #endregion
}