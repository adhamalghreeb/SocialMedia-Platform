using Blog_Project.CORE.Models.Domain;

namespace Blog_Project.Repositories.Interface
{
    public interface IImageRepositery
    {
        Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAllImages();
    }
}
