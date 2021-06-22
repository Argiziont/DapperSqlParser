namespace DapperSqlParser.TestRepository.Service.DapperExecutor
{
    public interface IDapperExecutorFactory
    {
        public IDapperExecutor<TIn, TOut> CreateDapperExecutor<TIn, TOut>() 
            where TOut : class 
            where TIn : class;

        public IDapperExecutor<TIn> CreateDapperExecutor<TIn>()
            where TIn : class;

    }
}