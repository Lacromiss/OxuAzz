using AutoMapper;
using OxuAzz.Dtos.CategoryDto;
using OxuAzz.Models;

namespace OxuAzz.Profils
{
    public class CategoryMapProfile: Profile
    {
        public CategoryMapProfile()
        {
            CreateMap<CategoryPostDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryGetDto>();
        }
    }
}
