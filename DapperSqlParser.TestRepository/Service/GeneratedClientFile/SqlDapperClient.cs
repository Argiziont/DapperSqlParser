using DapperSqlParser.TestRepository.Service.DapperExecutor;
using DapperSqlParser.TestRepository.Service.DapperExecutor.Extensions;

namespace DapperSqlParser.TestRepository.Service.GeneratedClientFile
{

	#region Sp_GetAllCategories
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_GetAllCategoriesOutput
	{
		[Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int Id { get; set; }

		[Newtonsoft.Json.JsonProperty("Name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Url { get; set; }


	}

	public class Sp_GetAllCategories
	{
		private readonly IDapperExecutor<EmptyInputParams, Sp_GetAllCategoriesOutput> _dapperExecutor;

		public Sp_GetAllCategories(IDapperExecutor<EmptyInputParams, Sp_GetAllCategoriesOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Sp_GetAllCategoriesOutput>> Execute()
		{
			return _dapperExecutor.ExecuteJsonAsync("Sp_GetAllCategories");
		}


	}

	#endregion

	#region Sp_GetAllProducts
	public class Sp_GetAllProductsOutput
	{
		[Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.Default)]
		public System.Int32 Id { get; set; }

		[Newtonsoft.Json.JsonProperty("ExternalId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String ExternalId { get; set; }

		[Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Title { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Url { get; set; }

		[Newtonsoft.Json.JsonProperty("SyncDate", Required = Newtonsoft.Json.Required.Default)]
		public System.DateTime SyncDate { get; set; }

		[Newtonsoft.Json.JsonProperty("ProductState", Required = Newtonsoft.Json.Required.Default)]
		public System.Int32 ProductState { get; set; }

		[Newtonsoft.Json.JsonProperty("Description", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Description { get; set; }

		[Newtonsoft.Json.JsonProperty("Price", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Price { get; set; }


	}


	public class Sp_GetAllProducts
	{
		private readonly IDapperExecutor<EmptyInputParams, Sp_GetAllProductsOutput> _dapperExecutor;

		public Sp_GetAllProducts(IDapperExecutor<EmptyInputParams, Sp_GetAllProductsOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Sp_GetAllProductsOutput>> Execute()
		{
			return _dapperExecutor.ExecuteAsync("Sp_GetAllProducts");
		}


	}

	#endregion

	#region Sp_GetProductById
	public class Sp_GetProductByIdOutput
	{
		[Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.Default)]
		public System.Int32 Id { get; set; }

		[Newtonsoft.Json.JsonProperty("ExternalId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String ExternalId { get; set; }

		[Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Title { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Url { get; set; }

		[Newtonsoft.Json.JsonProperty("SyncDate", Required = Newtonsoft.Json.Required.Default)]
		public System.DateTime SyncDate { get; set; }

		[Newtonsoft.Json.JsonProperty("ProductState", Required = Newtonsoft.Json.Required.Default)]
		public System.Int32 ProductState { get; set; }

		[Newtonsoft.Json.JsonProperty("Description", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Description { get; set; }

		[Newtonsoft.Json.JsonProperty("Price", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Price { get; set; }


	}

	public class Sp_GetProductByIdInput
	{
		[Newtonsoft.Json.JsonProperty("productId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.Int32 ProductId { get; set; }


	}

	public class Sp_GetProductById
	{
		private readonly IDapperExecutor<Sp_GetProductByIdInput, Sp_GetProductByIdOutput> _dapperExecutor;

		public Sp_GetProductById(IDapperExecutor<Sp_GetProductByIdInput, Sp_GetProductByIdOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Sp_GetProductByIdOutput>> Execute(Sp_GetProductByIdInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_GetProductById", request);
		}


	}

	#endregion

	#region Sp_GetCategoryById
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_GetCategoryByIdOutput
	{
		[Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int Id { get; set; }

		[Newtonsoft.Json.JsonProperty("Name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Url { get; set; }


	}
	[JsonWrapper("@jsonInput")]
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_GetCategoryByIdInput
	{
		[Newtonsoft.Json.JsonProperty("categoryId", Required = Newtonsoft.Json.Required.Always)]
		public int CategoryId { get; set; }


	}
	public class Sp_GetCategoryById
	{
		private readonly IDapperExecutor<Sp_GetCategoryByIdInput, Sp_GetCategoryByIdOutput> _dapperExecutor;

		public Sp_GetCategoryById(IDapperExecutor<Sp_GetCategoryByIdInput, Sp_GetCategoryByIdOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Sp_GetCategoryByIdOutput>> Execute(Sp_GetCategoryByIdInput request)
		{
			return _dapperExecutor.ExecuteJsonAsync("Sp_GetCategoryById", request);
		}


	}

	#endregion

	#region Sp_InsertProduct

	public class Sp_InsertProductInput
	{
		[Newtonsoft.Json.JsonProperty("ExternalId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String ExternalId { get; set; }

		[Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Title { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Url { get; set; }

		[Newtonsoft.Json.JsonProperty("SyncDate", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.DateTime SyncDate { get; set; }

		[Newtonsoft.Json.JsonProperty("ProductState", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.Int32 ProductState { get; set; }

		[Newtonsoft.Json.JsonProperty("Description", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Description { get; set; }

		[Newtonsoft.Json.JsonProperty("Price", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String Price { get; set; }


	}

	public class Sp_InsertProduct
	{
		private readonly IDapperExecutor<Sp_InsertProductInput> _dapperExecutor;

		public Sp_InsertProduct(IDapperExecutor<Sp_InsertProductInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_InsertProductInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_InsertProduct", request);
		}


	}

	#endregion

	#region Sp_InsertCategory

	[JsonWrapper("@jsonInput")]
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_InsertCategoryInput
	{
		[Newtonsoft.Json.JsonProperty("Name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Name { get; set; }

		[Newtonsoft.Json.JsonProperty("Url", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string Url { get; set; }


	}
	public class Sp_InsertCategory
	{
		private readonly IDapperExecutor<Sp_InsertCategoryInput> _dapperExecutor;

		public Sp_InsertCategory(IDapperExecutor<Sp_InsertCategoryInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_InsertCategoryInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_InsertCategory", request);
		}


	}

	#endregion

	#region Sp_UpdateProductTitleById

	public class Sp_UpdateProductTitleByIdInput
	{
		[Newtonsoft.Json.JsonProperty("productId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.Int32 ProductId { get; set; }

		[Newtonsoft.Json.JsonProperty("productTitle", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.String ProductTitle { get; set; }


	}

	public class Sp_UpdateProductTitleById
	{
		private readonly IDapperExecutor<Sp_UpdateProductTitleByIdInput> _dapperExecutor;

		public Sp_UpdateProductTitleById(IDapperExecutor<Sp_UpdateProductTitleByIdInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_UpdateProductTitleByIdInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_UpdateProductTitleById", request);
		}


	}

	#endregion

	#region Sp_UpdateCategoryNameById

	[JsonWrapper("@jsonInput")]
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_UpdateCategoryNameByIdInput
	{
		[Newtonsoft.Json.JsonProperty("categoryId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public int CategoryId { get; set; }

		[Newtonsoft.Json.JsonProperty("categoryName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
		public string CategoryName { get; set; }


	}
	public class Sp_UpdateCategoryNameById
	{
		private readonly IDapperExecutor<Sp_UpdateCategoryNameByIdInput> _dapperExecutor;

		public Sp_UpdateCategoryNameById(IDapperExecutor<Sp_UpdateCategoryNameByIdInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_UpdateCategoryNameByIdInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_UpdateCategoryNameById", request);
		}


	}

	#endregion

	#region Sp_DeleteCategoryById

	[JsonWrapper("@jsonInput")]
	[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.4.0 (Newtonsoft.Json v13.0.0.0)")]
	public partial class Sp_DeleteCategoryByIdInput
	{
		[Newtonsoft.Json.JsonProperty("categoryId", Required = Newtonsoft.Json.Required.Always)]
		public int CategoryId { get; set; }


	}
	public class Sp_DeleteCategoryById
	{
		private readonly IDapperExecutor<Sp_DeleteCategoryByIdInput> _dapperExecutor;

		public Sp_DeleteCategoryById(IDapperExecutor<Sp_DeleteCategoryByIdInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_DeleteCategoryByIdInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_DeleteCategoryById", request);
		}


	}

	#endregion

	#region Sp_DeleteProductById

	public class Sp_DeleteProductByIdInput
	{
		[Newtonsoft.Json.JsonProperty("productId", Required = Newtonsoft.Json.Required.DisallowNull)]
		public System.Int32 ProductId { get; set; }


	}

	public class Sp_DeleteProductById
	{
		private readonly IDapperExecutor<Sp_DeleteProductByIdInput> _dapperExecutor;

		public Sp_DeleteProductById(IDapperExecutor<Sp_DeleteProductByIdInput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}

		public System.Threading.Tasks.Task Execute(Sp_DeleteProductByIdInput request)
		{
			return _dapperExecutor.ExecuteAsync("Sp_DeleteProductById", request);
		}


	}

	#endregion
}
