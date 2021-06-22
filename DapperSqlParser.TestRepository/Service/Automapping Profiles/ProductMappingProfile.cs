using AutoMapper;
using DapperSqlParser.TestRepository.Models;
using DapperSqlParser.TestRepository.Service.GeneratedClientFile;

namespace DapperSqlParser.TestRepository.Service.Automapping_Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Sp_GetAllProductsOutput, Product>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.SyncDate,
                    opt => opt.MapFrom(src => src.SyncDate))
                .ForMember(dest => dest.ProductState,
                    opt => opt.MapFrom(src => src.ProductState))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price));

            CreateMap<Sp_GetProductByIdOutput, Product>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.SyncDate,
                    opt => opt.MapFrom(src => src.SyncDate))
                .ForMember(dest => dest.ProductState,
                    opt => opt.MapFrom(src => src.ProductState))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price));

            CreateMap<Product, Sp_InsertProductInput>()
                .ForMember(dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.SyncDate,
                    opt => opt.MapFrom(src => src.SyncDate))
                .ForMember(dest => dest.ProductState,
                    opt => opt.MapFrom(src => src.ProductState))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price));
        }
    }
}