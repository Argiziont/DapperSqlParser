using Microsoft.Extensions.Configuration;

namespace DapperSqlParser.TestRepository.Service.DapperExecutor
{
    public class DapperExecutorFactory : IDapperExecutorFactory
    {
        private readonly string _connectionString;

        public DapperExecutorFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("UserDb");
        }

        public IDapperExecutor<TIn, TOut> CreateDapperExecutor<TIn, TOut>()
            where TOut : class
            where TIn : class
        {
            return new DapperExecutor<TIn, TOut>(_connectionString);
        }

        public IDapperExecutor<TIn> CreateDapperExecutor<TIn>()
            where TIn : class
        {
            return new DapperExecutor<TIn>(_connectionString);
        }
    }
}