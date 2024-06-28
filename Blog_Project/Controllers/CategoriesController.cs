using AutoMapper;
using Azure;
using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Blog_Project.Models.DTO;
using Blog_Project.Repositories.Interface;
using Blog_Project.Repository;
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
        
        

        public IMapper Mapper { get; }
        public IUnitOfWork UnitOfWork { get; }

        public CategoriesController(IMapper mapper, IBaseRepository<Category> baseRepository, IUnitOfWork unitOfWork)
        {
            
            Mapper = mapper;
            
            UnitOfWork = unitOfWork;
        }

        [HttpPost]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreatCategory(CreateCategoryRequestDto requset)
        {
            var category = new Category { 
                Name = requset.Name,
                UrlHandle = requset.UrlHandle,
            };

            await UnitOfWork.Categories.Add(category);
            UnitOfWork.Complete();

            var response = Mapper.Map<CategoryDto>(category);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories( // modify
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize
            )
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);

            var response = Mapper.Map<List<CategoryDto>>(categories);

            return Ok(response);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var response = await UnitOfWork.Categories.GetById(id);
            if(response is null)
            {
                return NotFound();
            }

            
            var category = Mapper.Map<CategoryDto>(response);


            return Ok(category);
        }

        [HttpPut]
        [Route("{id:guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory(Guid id ,UpdateCategoryRequest category)
        {
            var cat = new Category { Id = id, Name = category.Name, UrlHandle = category.UrlHandle };

            var response = await UnitOfWork.Categories.Update(cat);
            if(response is null)
            {
                return NotFound();
            }
            UnitOfWork.Complete();

            var categoryDto = Mapper.Map<CategoryDto>(cat);
            categoryDto.Id = id;

            return Ok(categoryDto);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var response = await UnitOfWork.Categories.GetById(id);
            if (response is null)
            {
                return NotFound();
            }

            UnitOfWork.Categories.Delete(response);
            UnitOfWork.Complete();

            var respone = Mapper.Map<CategoryDto>(response);

            return Ok(respone);
        }
    }
}
