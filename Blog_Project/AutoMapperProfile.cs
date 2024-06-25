using AutoMapper;
using Blog_Project.Models.Domain;
using Blog_Project.Models.DTO;

namespace Blog_Project
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<BlogPost, BlogPostDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ReverseMap();

            CreateMap<BlogPost, BlogPostDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ReverseMap();

            CreateMap<BlogPost, BlogPostDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ReverseMap();

            CreateMap<Category, CategoryDto>()
            .ReverseMap();

            CreateMap<Category, UpdateCategoryRequest>()
                .ReverseMap();


        }

    }
}
