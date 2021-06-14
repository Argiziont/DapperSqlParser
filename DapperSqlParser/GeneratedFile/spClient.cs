namespace SpClient 
{

	#region sp_UpdateProductState

	public class sp_UpdateProductStateInput 
	{
		[Newtonsoft.Json.JsonProperty("productId")] public System.Int32 ProductId {get; set;} 

		[Newtonsoft.Json.JsonProperty("productState")] public System.Int32 ProductState {get; set;} 

	}

	public class sp_UpdateProductState 
	{
		private readonly IDapperExecutor<sp_UpdateProductStateInput> _dapperExecutor;
		public sp_UpdateProductState(IDapperExecutor<sp_UpdateProductStateInput>dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}
		public System.Threading.Tasks.Task Execute(sp_UpdateProductStateInput request)
		{
			return _dapperExecutor.ExecuteAsync("sp_UpdateProductState", request);
		}
	}

	#endregion

	#region sp_CountProductsWithCompany
	public class sp_CountProductsWithCompanyOutput 
	{
		[Newtonsoft.Json.JsonProperty("Result")] public System.Int32 Result {get; set;} 

	}

	public class sp_CountProductsWithCompanyInput 
	{
		[Newtonsoft.Json.JsonProperty("companyId")] public System.Int32 CompanyId {get; set;} 

	}

	public class sp_CountProductsWithCompany 
	{
		private readonly IDapperExecutor<sp_CountProductsWithCompanyInput, sp_CountProductsWithCompanyOutput> _dapperExecutor;
		public sp_CountProductsWithCompany(IDapperExecutor<sp_CountProductsWithCompanyInput, sp_CountProductsWithCompanyOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}
		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<sp_CountProductsWithCompanyOutput>> Execute(sp_CountProductsWithCompanyInput request)
		{
			return _dapperExecutor.ExecuteAsync("sp_CountProductsWithCompany", request);
		}
	}

	#endregion

	#region sp_GetAllProducts
	public class sp_GetAllProductsOutput 
	{
		[Newtonsoft.Json.JsonProperty("Id")] [System.ComponentModel.DataAnnotations.Required()] public System.Int32 Id {get; set;} 

		[Newtonsoft.Json.JsonProperty("CompanyId")] public System.Int32 CompanyId {get; set;} 

		[Newtonsoft.Json.JsonProperty("ExternalId")] public System.String ExternalId {get; set;} 

		[Newtonsoft.Json.JsonProperty("Title")] public System.String Title {get; set;} 

		[Newtonsoft.Json.JsonProperty("Url")] public System.String Url {get; set;} 

		[Newtonsoft.Json.JsonProperty("SyncDate")] [System.ComponentModel.DataAnnotations.Required()] public System.DateTime SyncDate {get; set;} 

		[Newtonsoft.Json.JsonProperty("ExpirationDate")] [System.ComponentModel.DataAnnotations.Required()] public System.DateTime ExpirationDate {get; set;} 

		[Newtonsoft.Json.JsonProperty("ProductState")] [System.ComponentModel.DataAnnotations.Required()] public System.Int32 ProductState {get; set;} 

		[Newtonsoft.Json.JsonProperty("Description")] public System.String Description {get; set;} 

		[Newtonsoft.Json.JsonProperty("Price")] public System.String Price {get; set;} 

		[Newtonsoft.Json.JsonProperty("KeyWords")] public System.String KeyWords {get; set;} 

		[Newtonsoft.Json.JsonProperty("JsonData")] public System.String JsonData {get; set;} 

		[Newtonsoft.Json.JsonProperty("JsonDataSchema")] public System.String JsonDataSchema {get; set;} 

	}


	public class sp_GetAllProducts 
	{
		private readonly IDapperExecutor<EmptyInputParams, sp_GetAllProductsOutput> _dapperExecutor;
		public sp_GetAllProducts(IDapperExecutor<EmptyInputParams, sp_GetAllProductsOutput> dapperExecutor)
		{
			this._dapperExecutor = dapperExecutor;
		}
		public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<sp_GetAllProductsOutput>> Execute()
		{
			return _dapperExecutor.ExecuteAsync("sp_GetAllProducts");
		}
	}

	#endregion
}