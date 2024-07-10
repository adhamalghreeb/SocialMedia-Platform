using Blog_Project.CORE;
using Blog_Project.CORE.Models.DTO;
using Blog_Project.Migrations;
using Blog_Project.CORE.Models.Domain;
using Blog_Project.Repositories.implementation;
using Blog_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        
        public IUnitOfWork UnitOfWork { get; }
        

        public ImageController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await UnitOfWork.BlogImages.FindAllAsync(b => b.Title.Contains(""));
            var respone = new List<BlogImageDto>();

            foreach (var blogimage in images)
            {
                respone.Add(new BlogImageDto
                {
                    Id = blogimage.Id,
                    Title = blogimage.Title,
                    DateCreated = blogimage.DateCreated,
                    FileExtension = blogimage.FileExtension,
                    FileName = blogimage.FileName,
                    Url = blogimage.Url
                });
            }

            return Ok(respone);
        }
        [HttpPost]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string filename, [FromForm] string title)
        {
            if(ModelState.IsValid)
            {
                var blogimage = new BlogImage {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = filename,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                blogimage = await UnitOfWork.BlogImages.UploadImage(file, blogimage);
                UnitOfWork.Complete();
                var response = new BlogImageDto
                {
                    Id = blogimage.Id,
                    Title = blogimage.Title,
                    DateCreated = blogimage.DateCreated,
                    FileExtension = blogimage.FileExtension,
                    FileName = blogimage.FileName,
                    Url = blogimage.Url
                };

                return Ok(response);

            }
            return BadRequest(ModelState);
        }
        

        
        private void ValidateFileUpload(IFormFile file)
        {
            // check format
            var allowedFormats = new[] { ".jpg", ".png", ".jpeg" };

            if (!allowedFormats.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }

            // check size
            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size exceeds the limit");
            }
        }
    }
}
