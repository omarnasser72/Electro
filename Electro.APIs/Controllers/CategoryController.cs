using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;
using Electro.Core.Interfaces;
using Electro.Repository.UnitOfWork;
using Electro.Repository.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Electro.APIs.Controllers
{
    [MyAuthorizer("Admin")]
    public class CategoryController : APIBaseController
    {
        private readonly UnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(UnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO?>>> GetAllCateogries()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAll();
            var categoriesDTO = _mapper.Map<IEnumerable<Category?>, IEnumerable<CategoryDTO?>>(categories);
            return Ok(new
            {
                Success = true,
                categories = categoriesDTO
            });
        }
        [HttpGet]
        public async Task<ActionResult<CategoryDTO?>> GetCateogry([FromQuery] int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(id);

            if (category == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Category Not found."
                });
            var categoryDTO = _mapper.Map<Category, CategoryDTO>(category);
            return Ok(new
            {
                Success = true,
                category = categoryDTO
            });
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<CategoryModel, Category>(categoryModel);
                var categoryExist = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(category); //.GetCategoryByNameAsync(category);
                if (categoryExist != null)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Category already exists."
                    });
                await _unitOfWork.CategoryRepository.AddAsync(category);
                if (await _unitOfWork.Complete())
                {
                    var categoryDTO = _mapper.Map<Category, CategoryDTO>(category);
                    return Ok(new
                    {
                        Success = true,
                        category = categoryDTO
                    });
                }
            }

            return BadRequest(new
            {
                Success = false,
                Message = "failed to add"
            });
        }

        [HttpPut]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory([FromQuery] int id, [FromBody] CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<CategoryModel, Category>(categoryModel);
                var categoryExist = await _unitOfWork.CategoryRepository.GetById(id);
                if (categoryExist == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Category doesn't exist."
                    });

                categoryExist.Name = category.Name;
                _unitOfWork.CategoryRepository.Update(categoryExist);

                if (await _unitOfWork.Complete())
                {
                    var categoryDTO = _mapper.Map<Category, CategoryDTO>(category);
                    return Ok(new
                    {
                        Success = false,
                        category = categoryDTO
                    });
                }
            }
            return BadRequest(new
            {
                Success = false,
                Message = "failed to update"
            });
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCategory([FromQuery] int id)
        {
            _unitOfWork.CategoryRepository.Delete(id);

            if (await _unitOfWork.Complete())
                return Ok(new
                {
                    Success = false,
                    Message = "Deleted Successfully"
                });

            return BadRequest(new
            {
                Success = false,
                Message = "failed to delete"
            });
        }
    }
}
