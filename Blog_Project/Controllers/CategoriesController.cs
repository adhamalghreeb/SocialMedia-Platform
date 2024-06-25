using AutoMapper;
using Azure;
using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Models.DTO;
using Blog_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public IMapper Mapper { get; }

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            Mapper = mapper;
        }

        [HttpPost]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreatCategory(CreateCategoryRequestDto requset)
        {
            var category = new Category { 
                Name = requset.Name,
                UrlHandle = requset.UrlHandle,
            };

            await categoryRepository.CreateAsync(category);

            var response = Mapper.Map<CategoryDto>(category);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize
            )
        {
            var categories = await categoryRepository.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);

            var response = Mapper.Map<List<CategoryDto>>(categories);

            return Ok(response);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var response = await categoryRepository.GetByIdAsync(id);
            if(response is null)
            {
                return NotFound();
            }

            
            var category = Mapper.Map<CategoryDto>(response);


            return Ok(category);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory(Guid id ,UpdateCategoryRequest category)
        {
            var cat = new Category { Id = id, Name = category.Name, UrlHandle = category.UrlHandle };

            var response = await categoryRepository.Update(cat);
            if(response is null)
            {
                return NotFound();
            }

            var categoryDto = Mapper.Map<CategoryDto>(category);
            categoryDto.Id = id;

            return Ok(categoryDto);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await categoryRepository.DeleteCategory(id);
            if (category is null) { return NotFound(); }

            var respone = Mapper.Map<CategoryDto>(category);

            return Ok(respone);
        }
    }
}
