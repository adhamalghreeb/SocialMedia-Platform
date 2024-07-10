using AutoMapper;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.CORE.Models.DTO;

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

            CreateMap<CommentDTO, Comment>()
                .ReverseMap()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Comment, CreateCommentRequest>()
                .ReverseMap();

            CreateMap<EditCommentRequest, Comment>()
                .ReverseMap();

            CreateMap<follow, FollowerDTO>()
                .ReverseMap();

            CreateMap<AppUser, FollowerDTO>()
            .ReverseMap();

        }

    }
}
