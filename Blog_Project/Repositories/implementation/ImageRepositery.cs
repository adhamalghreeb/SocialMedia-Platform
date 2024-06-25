using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Repositories.implementation
{
    public class ImageRepositery : IImageRepositery
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly appDBcontext appDBcontext;

        public ImageRepositery(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, appDBcontext appDBcontext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.appDBcontext = appDBcontext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllImages()
        {
            return await appDBcontext.blogImages.ToListAsync();
        }

        public async Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            
            await appDBcontext.blogImages.AddAsync( blogImage );
            await appDBcontext.SaveChangesAsync();

            return blogImage;
        }
    }
}
