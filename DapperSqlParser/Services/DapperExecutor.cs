using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperSqlParser.Extensions;
using Newtonsoft.Json;
using SpClient;

namespace DapperSqlParser.Services
{
    public class DapperExecutor<TInParams> : IDapperExecutor<TInParams>
        where TInParams : class
    {
        private readonly string _connectionString;

        public DapperExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task ExecuteAsync(string spName, TInParams inputParams)
        {
            await using var connection = new SqlConnection(_connectionString);

            /*
             *  If input is json so we must to know how to deserialize this
             *  Dapper requires input names to be set
             *  We pass this param name through  JsonWrapperAttribute
             *  And create dynamic dictionary wrapper for our object
             */
            if (typeof(TInParams).IsDefined(typeof(JsonWrapperAttribute), true))
            {
                var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    {  JsonWrapperAttributeExtensions.GetAttributeCustom<TInParams>().StoreProcedureJsonInputName, JsonConvert.SerializeObject(inputParams) }
                });

                await connection.ExecuteAsync(spName, param: parameters, commandType: CommandType.StoredProcedure);
                return;
            }

            await connection.ExecuteAsync(spName, param: inputParams, commandType: CommandType.StoredProcedure);
        }

    }

    public class DapperExecutor<TInParams, TOutParams> : IDapperExecutor<TInParams, TOutParams> 
        where TOutParams : class
        where TInParams : class
    {
        private readonly string _connectionString;

        public DapperExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<IEnumerable<TOutParams>> ExecuteAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<TOutParams>(spName, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TOutParams>> ExecuteJsonAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);

            return await Task.FromResult(connection.QueryJson<TOutParams>(spName, commandType: CommandType.StoredProcedure,
                buffered: false));
        }

        async Task<IEnumerable<TOutParams>> IDapperExecutor<TInParams, TOutParams>.ExecuteAsync(string spName, TInParams inputParams)
        {
            await using var connection = new SqlConnection(_connectionString);

            if (typeof(TInParams).IsDefined(typeof(JsonWrapperAttribute), true))
            {
                var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    {  JsonWrapperAttributeExtensions.GetAttributeCustom<TInParams>().StoreProcedureJsonInputName, JsonConvert.SerializeObject(inputParams) }
                });

                return await connection.QueryAsync<TOutParams>(spName, param: parameters, commandType: CommandType.StoredProcedure);
            }

            if ((typeof(TInParams) == typeof(EmptyInputParams))|| inputParams.Equals(default))
            {
                return await ExecuteAsync(spName);
            }

         

            return await connection.QueryAsync<TOutParams>(spName, param: inputParams, commandType: CommandType.StoredProcedure);
        }

        async Task<IEnumerable<TOutParams>> IDapperExecutor<TInParams, TOutParams>.ExecuteJsonAsync(string spName, TInParams inputParams)
        {
            await using var connection = new SqlConnection(_connectionString);

            if (typeof(TInParams).IsDefined(typeof(JsonWrapperAttribute), true))
            {
                var parameters = new DynamicParameters(new Dictionary<string, object>
                {
                    { JsonWrapperAttributeExtensions.GetAttributeCustom<TInParams>().StoreProcedureJsonInputName, JsonConvert.SerializeObject(inputParams) }
                });

                return await Task.FromResult(connection.QueryJson<TOutParams>(spName, param: parameters, commandType: CommandType.StoredProcedure,
                    buffered: false));
            }

            if ((typeof(TInParams) == typeof(EmptyInputParams)) || inputParams.Equals(default))
            {
                return await ExecuteJsonAsync(spName);
            }

            return await Task.FromResult(connection.QueryJson<TOutParams>(spName, param: inputParams, commandType: CommandType.StoredProcedure,
                buffered: false));
        }

       
    }


}