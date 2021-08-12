using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DapperSqlParser.TestRepository.GraphQlServices.PlaygroundService;
using HotChocolate;

namespace DapperSqlParser.TestRepository.GraphQlServices
{
    public class QueryService
    {
        public async Task<ICollection<PlaygroundService.Product>> GetAllProductsAsync(
            [Service] ProductsClient service, CancellationToken cancellationToken)
        {
            return await service.GetAllAsync(cancellationToken);



        }
        public async Task<Product> GetProductByIdAsync(
            [Service] ProductsClient service, int id, CancellationToken cancellationToken)
        {
            return await service.GetByIdAsync(id, cancellationToken);
        }
    }
}