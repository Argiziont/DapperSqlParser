using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperSqlParser.TestRepository.Service.DapperExecutor
{
    public interface IDapperExecutor<in TInParams>
    {
        Task ExecuteAsync(string spName, TInParams inputParams);
    }

    public interface IDapperExecutor<in TInParams, TOutParams>
        where TOutParams : class
        where TInParams : class
    {
        Task<IEnumerable<TOutParams>> ExecuteAsync(string spName, TInParams inputParams);
        Task<IEnumerable<TOutParams>> ExecuteAsync(string spName);
        Task<IEnumerable<TOutParams>> ExecuteJsonAsync(string spName);
        Task<IEnumerable<TOutParams>> ExecuteJsonAsync(string spName, TInParams inputParams);
    }


    public class EmptyInputParams
    {
    }
}