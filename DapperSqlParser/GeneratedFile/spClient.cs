using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using DapperSqlParser.Dapper_Services;
using Newtonsoft.Json;
using SpClient;

namespace DapperSqlParser.GeneratedFile
{
    #region Sp_GetAllCategories

    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetAllCategoriesOutput
    {
        [JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
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
            return _dapperExecutor.ExecuteJsonAsync("Sp_GetAllCategories");
        }
    }

    #endregion


    #region Sp_GetAllProducts

    public class Sp_GetAllProductsOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }
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
            return _dapperExecutor.ExecuteAsync("Sp_GetAllProducts");
        }
    }

    #endregion


    #region Sp_GetProductById

    public class Sp_GetProductByIdOutput
    {
        [JsonProperty("Id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.Default)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ProductState", Required = Required.Default)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }
    }

    public class Sp_GetProductByIdInput
    {
        [JsonProperty("productId", Required = Required.DisallowNull)]
        public int ProductId { get; set; }
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
    }

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_GetCategoryByIdInput
    {
        [JsonProperty("categoryId", Required = Required.Always)]
        public int CategoryId { get; set; }
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


    #region Sp_InsertProduct

    public class Sp_InsertProductInput
    {
        [JsonProperty("ExternalId", Required = Required.DisallowNull)]
        public string ExternalId { get; set; }

        [JsonProperty("Title", Required = Required.DisallowNull)]
        public string Title { get; set; }

        [JsonProperty("Url", Required = Required.DisallowNull)]
        public string Url { get; set; }

        [JsonProperty("SyncDate", Required = Required.DisallowNull)]
        public DateTime SyncDate { get; set; }

        [JsonProperty("ProductState", Required = Required.DisallowNull)]
        public int ProductState { get; set; }

        [JsonProperty("Description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        [JsonProperty("Price", Required = Required.DisallowNull)]
        public string Price { get; set; }
    }

    public class Sp_InsertProduct
    {
        private readonly IDapperExecutor<Sp_InsertProductInput> _dapperExecutor;

        public Sp_InsertProduct(IDapperExecutor<Sp_InsertProductInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_InsertProductInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_InsertProduct", request);
        }
    }

    #endregion


    #region Sp_InsertCategory

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_InsertCategoryInput
    {
        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public class Sp_InsertCategory
    {
        private readonly IDapperExecutor<Sp_InsertCategoryInput> _dapperExecutor;

        public Sp_InsertCategory(IDapperExecutor<Sp_InsertCategoryInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_InsertCategoryInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_InsertCategory", request);
        }
    }

    #endregion


    #region Sp_UpdateProductTitleById

    public class Sp_UpdateProductTitleByIdInput
    {
        [JsonProperty("productId", Required = Required.DisallowNull)]
        public int ProductId { get; set; }

        [JsonProperty("productTitle", Required = Required.DisallowNull)]
        public string ProductTitle { get; set; }
    }

    public class Sp_UpdateProductTitleById
    {
        private readonly IDapperExecutor<Sp_UpdateProductTitleByIdInput> _dapperExecutor;

        public Sp_UpdateProductTitleById(IDapperExecutor<Sp_UpdateProductTitleByIdInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_UpdateProductTitleByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_UpdateProductTitleById", request);
        }
    }

    #endregion


    #region Sp_UpdateCategoryNameById

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_UpdateCategoryNameByIdInput
    {
        [JsonProperty("categoryId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int CategoryId { get; set; }

        [JsonProperty("categoryName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CategoryName { get; set; }
    }

    public class Sp_UpdateCategoryNameById
    {
        private readonly IDapperExecutor<Sp_UpdateCategoryNameByIdInput> _dapperExecutor;

        public Sp_UpdateCategoryNameById(IDapperExecutor<Sp_UpdateCategoryNameByIdInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_UpdateCategoryNameByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_UpdateCategoryNameById", request);
        }
    }

    #endregion


    #region Sp_DeleteCategoryById

    [JsonWrapper("@jsonInput")]
    [GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Sp_DeleteCategoryByIdInput
    {
        [JsonProperty("categoryId", Required = Required.Always)]
        public int CategoryId { get; set; }
    }

    public class Sp_DeleteCategoryById
    {
        private readonly IDapperExecutor<Sp_DeleteCategoryByIdInput> _dapperExecutor;

        public Sp_DeleteCategoryById(IDapperExecutor<Sp_DeleteCategoryByIdInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_DeleteCategoryByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_DeleteCategoryById", request);
        }
    }

    #endregion


    #region Sp_DeleteProductById

    public class Sp_DeleteProductByIdInput
    {
        [JsonProperty("productId", Required = Required.DisallowNull)]
        public int ProductId { get; set; }
    }

    public class Sp_DeleteProductById
    {
        private readonly IDapperExecutor<Sp_DeleteProductByIdInput> _dapperExecutor;

        public Sp_DeleteProductById(IDapperExecutor<Sp_DeleteProductByIdInput> dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public Task Execute(Sp_DeleteProductByIdInput request)
        {
            return _dapperExecutor.ExecuteAsync("Sp_DeleteProductById", request);
        }
    }

    #endregion
}