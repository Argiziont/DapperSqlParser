using System.Collections.Generic;
using System.Threading.Tasks;
using DapperSqlParser.TestRepository.Models;

namespace DapperSqlParser.TestRepository.Service.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<Category> GetByIdAsync(int categoryId);
        public Task DeleteByIdAsync(int categoryId);
        public Task InsertAsync(Category category);
        public Task UpdateNameByIdAsync(int categoryId, string categoryName);
    }
}