using Blog_Project.Models.Domain;
using Blog_Project.Repository;

namespace Blog_Project.Repositories.Interface
{
    public interface IBlogImage : IBaseRepository<BlogImage>
    {
        public Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage);
    }
}
