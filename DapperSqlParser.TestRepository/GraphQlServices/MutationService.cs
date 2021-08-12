using System.Threading;
using System.Threading.Tasks;
using DapperSqlParser.TestRepository.GraphQlServices.PlaygroundService;
using HotChocolate;

namespace DapperSqlParser.TestRepository.GraphQlServices
{
    public class MutationService
    {
        public async Task<Product> AddProductAsync(
            [Service] ProductsClient service, Product product, CancellationToken cancellationToken)
        {
            await service.InsertAsync(product, cancellationToken);
            return product;
        }
    }
}