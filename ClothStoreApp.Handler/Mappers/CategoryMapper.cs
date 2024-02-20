using AutoMapper;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Categories.Dtos;

namespace ClothStoreApp.Handler.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
