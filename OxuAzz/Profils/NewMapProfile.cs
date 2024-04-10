using AutoMapper;
using OxuAzz.Dtos.NewDto;
using OxuAzz.Models;

namespace OxuAzz.Profils
{
    public class NewMapProfile:Profile
    {
        public NewMapProfile()
        {
            CreateMap<New, NewGetDto>();
            CreateMap<NewPostDto, New>();
        }

    }
}
