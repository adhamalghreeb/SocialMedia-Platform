using Blog_Project.CORE.@interface;
using Blog_Project.CORE.Models.Domain;

namespace Blog_Project.CORE.@interface
{
    public interface IBlogImage : IBaseRepository<BlogImage>
    {
        public Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage);
    }
}
