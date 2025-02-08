using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;
using Electro.Repository.UnitOfWork;
using Electro.Repository.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Electro.APIs.Controllers
{
    [MyAuthorizer("Admin")]
    public class BrandController : APIBaseController
    {
        private readonly UnitOfWork<Brand> _unitOfWork;
        private readonly IMapper _mapper;

        public BrandController(UnitOfWork<Brand> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BrandDTO>?>> GetAllBrands()
        {
            var brands = await _unitOfWork.BrandRepository.GetAll();
            var brandsDTO = _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandDTO>>(brands);
            return Ok(new
            {
                Success = true,
                brands = brandsDTO
            });
        }

        [HttpGet]
        public async Task<ActionResult<BrandDTO?>> GetBrand([FromQuery] int id)
        {
            var brand = await _unitOfWork.BrandRepository.GetById(id);
            if (brand == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Brand Not found."
                });

            var brandDTO = _mapper.Map<Brand, BrandDTO>(brand);
            return Ok(new
            {
                Success = true,
                brand = brandDTO
            });
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> Add([FromBody] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                var brand = _mapper.Map<BrandModel, Brand>(brandModel);
                var brandExist = await _unitOfWork.BrandRepository.GetBrandByNameAsync(brand);
                if (brandExist != null)
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Brand already exists."
                    });

                await _unitOfWork.BrandRepository.AddAsync(brand);
                if (await _unitOfWork.Complete())
                {
                    var brandDTO = _mapper.Map<Brand, BrandDTO>(brand);
                    return Ok(new
                    {
                        Success = true,
                        brand = brandDTO
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
        public async Task<ActionResult<BrandDTO>> UpdateBrand([FromQuery] int id, [FromBody] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                var brand = _mapper.Map<BrandModel, Brand>(brandModel);
                var brandExist = await _unitOfWork.BrandRepository.GetById(id);
                if (brandExist == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Brand doesn't exist."
                    });

                brandExist.Name = brand.Name;
                _unitOfWork.BrandRepository.Update(brandExist);

                if (await _unitOfWork.Complete())
                {
                    var brandDTO = _mapper.Map<Brand, BrandDTO>(brand);
                    return Ok(new
                    {
                        Success = true,
                        brand = brandDTO
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
        public async Task<ActionResult> DeleteBrand([FromQuery] int id)
        {
            _unitOfWork.BrandRepository.Delete(id);

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
    }
}
