using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Repositories.Interface;
using Blog_Project.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blog_Project.Repositories.implementation
{
    public class BlogImageRepo : BaseRepository<BlogImage> ,  IBlogImage
    {
        private readonly appDBcontext appDBcontext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BlogImageRepo(appDBcontext appDBcontext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(appDBcontext)
        {
            this.appDBcontext = appDBcontext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;

            if (webHostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(webHostEnvironment));
            }
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }
        }

        public async Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await appDBcontext.blogImages.AddAsync(blogImage);
            

            return blogImage;
        }
    }
}