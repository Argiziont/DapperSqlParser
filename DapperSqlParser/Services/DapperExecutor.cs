﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SpClient;

namespace DapperSqlParser.Services
{
    public class DapperExecutor<TInParams> : IDapperExecutor<TInParams>
    {
        private readonly string _connectionString;

        public DapperExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task ExecuteAsync(string spName, TInParams inputParams)
        {
            await using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(spName, param: inputParams, commandType: CommandType.StoredProcedure);
        }
    }

    public class DapperExecutor<TInParams, TOutParams> : IDapperExecutor<TInParams, TOutParams>
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

        async Task<IEnumerable<TOutParams>> IDapperExecutor<TInParams, TOutParams>.ExecuteAsync(string spName, TInParams inputParams)
        {
            if ((typeof(TInParams) == typeof(EmptyInputParams))|| inputParams.Equals(default))
            {
                return await ExecuteAsync(spName);
            }
            await using var connection = new SqlConnection(_connectionString); 

            return await connection.QueryAsync<TOutParams>(spName, param: inputParams, commandType: CommandType.StoredProcedure);
        }
    }

}