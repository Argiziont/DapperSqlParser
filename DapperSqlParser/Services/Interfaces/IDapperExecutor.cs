// ReSharper disable InconsistentNaming
namespace SpClient
{
    public interface IDapperExecutor<in TInParams>
    {
        System.Threading.Tasks.Task ExecuteAsync(string spName, TInParams inputParams);

    }

    public interface IDapperExecutor<in TInParams, TOutParams> 
        where TOutParams : class
        where TInParams : class
    {
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TOutParams>> ExecuteAsync(string spName, TInParams inputParams);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TOutParams>> ExecuteAsync(string spName);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TOutParams>> ExecuteJsonAsync(string spName);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TOutParams>> ExecuteJsonAsync(string spName, TInParams inputParams);
    }


    public class EmptyInputParams { }

}