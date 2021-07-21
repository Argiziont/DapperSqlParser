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
    public class ProductRepository : IProductRepository
    {
        private readonly IDapperExecutorFactory _dapperExecutorFactory;
        private readonly IMapper _mapper;

        public ProductRepository(IDapperExecutorFactory dapperExecutorFactory, IMapper mapper)
        {
            _dapperExecutorFactory = dapperExecutorFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<EmptyInputParams, Sp_GetAllProductsOutput>();
            Sp_GetAllProducts spService = new Sp_GetAllProducts(executor);

            var executeResult = await spService.Execute();
            try
            {
                return _mapper.Map<IEnumerable<Sp_GetAllProductsOutput>, IEnumerable<Product>>(executeResult);
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

        public async Task<Product> GetByIdAsync(int productId)
        {
            var executor =
                _dapperExecutorFactory.CreateDapperExecutor<Sp_GetProductByIdInput, Sp_GetProductByIdOutput>();
            Sp_GetProductById spService = new Sp_GetProductById(executor);

            var executeResult = await spService.Execute(new Sp_GetProductByIdInput {ProductId = productId});
            try
            {
                return _mapper.Map<IEnumerable<Sp_GetProductByIdOutput>, IEnumerable<Product>>(executeResult).First();
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

        public async Task DeleteByIdAsync(int productId)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_DeleteProductByIdInput>();
            Sp_DeleteProductById spService = new Sp_DeleteProductById(executor);

            await spService.Execute(new Sp_DeleteProductByIdInput {ProductId = productId});
        }

        public async Task InsertAsync(Product product)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_InsertProductInput>();
            Sp_InsertProduct spService = new Sp_InsertProduct(executor);

            try
            {
                await spService.Execute(_mapper.Map<Product, Sp_InsertProductInput>(product));
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                if (autoMapperException.InnerException != null) throw autoMapperException.InnerException;
                // this will break your call stack
                // you may not know where the error is called and rather
                // want to clone the InnerException or throw a brand new Exception
            }
        }

        public async Task UpdateTitleByIdAsync(int productId, string productTitle)
        {
            var executor = _dapperExecutorFactory.CreateDapperExecutor<Sp_UpdateProductTitleByIdInput>();
            Sp_UpdateProductTitleById spService = new Sp_UpdateProductTitleById(executor);

            await spService.Execute(new Sp_UpdateProductTitleByIdInput
                {ProductId = productId, ProductTitle = productTitle});
        }
    }
}