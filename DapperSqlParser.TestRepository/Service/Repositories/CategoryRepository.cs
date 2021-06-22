using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DapperSqlParser.TestRepository.Models;
using DapperSqlParser.TestRepository.Service.DapperExecutor;
using DapperSqlParser.TestRepository.Service.GeneratedClientFile;
using DapperSqlParser.TestRepository.Service.Repositories.Interfaces;

namespace DapperSqlParser.TestRepository.Service.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly IDapperExecutorFactory _dapperExecutorFactory;
        private readonly IMapper _mapper;

        public CategoryRepository(IDapperExecutorFactory dapperExecutorFactory, IMapper mapper)
        {
            _dapperExecutorFactory = dapperExecutorFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<EmptyInputParams, Sp_GetAllCategoriesOutput>();
            var spService = new Sp_GetAllCategories(executor);

            var executeResult = await spService.Execute();
            try
            {
                return _mapper.Map<IEnumerable<Sp_GetAllCategoriesOutput>, IEnumerable<Category>>(executeResult);
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                if (autoMapperException.InnerException != null) throw autoMapperException.InnerException;
                // this will break your call stack
                // you may not know where the error is called and rather
                // want to clone the InnerException or throw a brand new Exception

                return null;
            }
        }

        public async Task<Category> GetByIdAsync(int categoryId)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_GetCategoryByIdInput, Sp_GetCategoryByIdOutput>();
            var spService = new Sp_GetCategoryById(executor);

            var executeResult = await spService.Execute(new Sp_GetCategoryByIdInput() { CategoryId = categoryId });
            try
            {
                return _mapper.Map<IEnumerable<Sp_GetCategoryByIdOutput>, IEnumerable<Category>>(executeResult).First();
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                if (autoMapperException.InnerException != null) throw autoMapperException.InnerException;
                // this will break your call stack
                // you may not know where the error is called and rather
                // want to clone the InnerException or throw a brand new Exception

                return null;
            }
        }

        public async Task DeleteByIdAsync(int categoryId)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_DeleteCategoryByIdInput>();
            var spService = new Sp_DeleteCategoryById(executor);

            await spService.Execute(new Sp_DeleteCategoryByIdInput() { CategoryId = categoryId });
        }

        public async Task InsertAsync(Category category)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_InsertCategoryInput>();
            var spService = new Sp_InsertCategory(executor);

            try
            {
                await spService.Execute(_mapper.Map<Category, Sp_InsertCategoryInput>(category));
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                if (autoMapperException.InnerException != null) throw autoMapperException.InnerException;
                // this will break your call stack
                // you may not know where the error is called and rather
                // want to clone the InnerException or throw a brand new Exception
            }
        }

        public async Task UpdateNameByIdAsync(int categoryId, string categoryName)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_UpdateCategoryNameByIdInput>();
            var spService = new Sp_UpdateCategoryNameById(executor);

            await spService.Execute(new Sp_UpdateCategoryNameByIdInput() { CategoryId = categoryId, CategoryName = categoryName });
        }
    }
}