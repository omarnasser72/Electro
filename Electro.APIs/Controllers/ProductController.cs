using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;
using Electro.Repository.UnitOfWork;
using Electro.Repository.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Electro.APIs.Controllers
{
    [MyAuthorizer]
    public class ProductController : APIBaseController
    {
        private readonly UnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(UnitOfWork<Product> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();
            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<Product>>(products);
            return Ok(new
            {
                Success = true,
                products = productsDTO
            });
        }
        [HttpGet]
        public async Task<ActionResult<ProductDTO?>> GetProduct([FromQuery] int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Product Not found."
                });
            var productDTO = _mapper.Map<Product, ProductDTO>(product);
            return Ok(new
            {
                Success = true,
                product = productDTO
            });
        }

        [MyAuthorizer("Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductModel, Product>(productModel);
                var productExist = await _unitOfWork.ProductRepository.GetProductByNameAsync(product);
                if (productExist != null)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Product already exists."
                    });
                await _unitOfWork.ProductRepository.AddAsync(product);
                if (await _unitOfWork.Complete())
                {
                    var productDTO = _mapper.Map<Product, ProductDTO>(product);
                    return Ok(new
                    {
                        Success = true,
                        product = productDTO
                    });
                }
            }
            return BadRequest(new
            {
                Success = false,
                Message = "failed to add"
            });
        }

        [MyAuthorizer("Admin")]
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateProduct([FromQuery] int id, [FromBody] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductModel, Product>(productModel);
                var productExist = await _unitOfWork.ProductRepository.GetById(id);
                if (productExist == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Product doesn't exist."
                    });

                productExist.Name = product.Name;
                _unitOfWork.ProductRepository.Update(productExist);

                if (await _unitOfWork.Complete())
                {
                    var productDTO = _mapper.Map<Product, ProductDTO>(product);
                    return Ok(new
                    {
                        Success = true,
                        product = productDTO
                    });
                }

            }
            return BadRequest(new
            {
                Success = false,
                Message = "failed to update"
            });
        }

        [MyAuthorizer("Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct([FromQuery] int id)
        {
            _unitOfWork.ProductRepository.Delete(id);

            if (await _unitOfWork.Complete())
                return Ok(new
                {
                    Success = true,
                    Message = "Deleted Successfully"
                });
            return BadRequest(new
            {
                Success = false,
                Message = "failed to delete"
            });
        }

        [HttpGet("GetProductsByCategoryName")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategoryName([FromQuery] string categoryName)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryName(categoryName);
            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return Ok(new
            {
                Success = true,
                products = productsDTO
            });
        }

        [HttpGet("GetProductsByBrandName")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByBrandName([FromQuery] string brandName)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByBrandName(brandName);
            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return Ok(new
            {
                Success = true,
                products = productsDTO
            });
        }
    }
}
