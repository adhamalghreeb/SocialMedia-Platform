using AutoMapper;
using Azure.Core;
using Blog_Project.Models.Domain;
using Blog_Project.Models.DTO;
using Blog_Project.Repositories.implementation;
using Blog_Project.Repositories.Interface;
using Blog_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BlogPostController : ControllerBase
    {
        
        public IMapper Mapper { get; }
        
        public IUnitOfWork UnitOfWork { get; }

        public BlogPostController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            
            Mapper = mapper;
            
            UnitOfWork = unitOfWork;
        }

        [HttpPost]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var item in request.Categories)
            {
                var exisit = await UnitOfWork.Categories.GetById(item);
                if(exisit is not null)
                {
                    blogPost.Categories.Add(exisit);
                }
            }

            blogPost = await UnitOfWork.BlogPosts.Add(blogPost);
            UnitOfWork.Complete();
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto 
                { Id = x.Id, Name = x.Name , UrlHandle = x.UrlHandle}).ToList()
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogPosts = await UnitOfWork.BlogPosts.FindAll(b => b.Title.Contains(""), new[] { "Categories" });
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
                });
            }
            return Ok(response);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            var blogPost = await UnitOfWork.BlogPosts.GetById(id , new string[] { "Categories" });
            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
            };

            return Ok(response);

        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogByUrl([FromRoute] string urlHandle)
        {
            var blogPost = await UnitOfWork.BlogPosts.GetByUrlAsync(urlHandle);
            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
            };

            return Ok(response);

        }

        [HttpPut]
        [Route("{id:guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlog(Guid id, UpdateBlogPostRequestDTO request)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await UnitOfWork.Categories.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var updatedBlogPost = await UnitOfWork.BlogPosts.Update(blogPost);

            if (updatedBlogPost == null)
            {
                return NotFound();
            }
            UnitOfWork.Complete();

            // Convert Domain model back to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> deleteBlog(Guid id)
        {
            var blogPost = await UnitOfWork.BlogPosts.GetById(id);
            if (blogPost is null)
            {
                return NotFound();
            }

            UnitOfWork.BlogPosts.Delete(blogPost);
            UnitOfWork.Complete();

            var response = new BlogPostDto
            {
                Id = id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle
            };

            return Ok(response);
        }
        

    }
}
