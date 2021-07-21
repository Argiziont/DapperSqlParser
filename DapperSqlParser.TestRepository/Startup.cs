using AutoMapper;
using DapperSqlParser.TestRepository.Service.Automapping_Profiles;
using DapperSqlParser.TestRepository.Service.DapperExecutor;
using DapperSqlParser.TestRepository.Service.Repositories;
using DapperSqlParser.TestRepository.Service.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DapperSqlParser.TestRepository
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //AutoMapper
            MapperConfiguration mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ProductMappingProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            mapperConfig.AssertConfigurationIsValid(); //Check if map profiles is valid

            //Dapper repository wrappers
            services.AddTransient<IDapperExecutorFactory, DapperExecutorFactory>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            // Register the Swagger services
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}