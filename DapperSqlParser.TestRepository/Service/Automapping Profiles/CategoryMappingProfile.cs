using AutoMapper;
using DapperSqlParser.TestRepository.Models;
using DapperSqlParser.TestRepository.Service.GeneratedClientFile;

namespace DapperSqlParser.TestRepository.Service.Automapping_Profiles
{
    public class CategoryMappingProfile:Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Sp_GetAllCategoriesOutput, Category>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url));

            CreateMap<Sp_GetCategoryByIdOutput, Category>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url));

            CreateMap<Category, Sp_InsertCategoryInput>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url));
        }
    }
}