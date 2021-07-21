using System.Collections.Generic;
using System.Threading.Tasks;
using DapperSqlParser.TestRepository.Models;

namespace DapperSqlParser.TestRepository.Service.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product> GetByIdAsync(int productId);
        public Task DeleteByIdAsync(int productId);
        public Task InsertAsync(Product product);
        public Task UpdateTitleByIdAsync(int productId, string productTitle);
    }
}