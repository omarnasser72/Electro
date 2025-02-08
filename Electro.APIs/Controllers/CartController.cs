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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Electro.APIs.Controllers
{
    [MyAuthorizer(new[] { "Customer", "Admin" })]
    public class CartController : APIBaseController
    {
        private readonly UnitOfWork<Cart> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CartController(UnitOfWork<Cart> unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        private async Task<ModelStateDictionary> CheckProductsQuantities(ICollection<CartProduct> cartProducts)
        {
            foreach (var item in cartProducts)
            {
                var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                if (item.Quantity > product.QuantityInStock)
                    ModelState.AddModelError($"Product{item.ProductId}", "can't have quantity more than that in stock");
            }
            return ModelState;
        }

        [MyAuthorizer("Admin")]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            var carts = await _unitOfWork.CartRepository.GetAll();
            var cartDTO = _mapper.Map<IEnumerable<Cart>, IEnumerable<CartDTO>>(carts);
            return Ok(new
            {
                Success = true,
                carts = cartDTO
            });
        }

        [HttpGet]
        public async Task<ActionResult<CartDTO?>> GetCart([FromQuery] string id)
        {
            var cart = await _unitOfWork.CartRepository.GetById(id);
            if (cart == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Cart Not found."
                });
            var cartDTO = _mapper.Map<Cart, CartDTO>(cart);
            return Ok(new
            {
                status = "Success",
                cart = cartDTO
            });
        }

        [HttpPost]
        public async Task<ActionResult<CartDTO>> Add([FromBody] CartModel cartModel)
        {
            if (ModelState.IsValid)
            {
                var customerExists = await _userManager.FindByIdAsync(cartModel.CustomerId.ToString());
                if (customerExists == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Customer does not exist."
                    });
                }

                var cart = _mapper.Map<CartModel, Cart>(cartModel);

                var cartExist = await _unitOfWork.CartRepository.GetById(cart.CustomerId);

                if (cartExist != null)
                    return await Update(cartModel.CustomerId, cartModel);

                if (cart.CartProducts?.Count > 0)
                {
                    var cartModelState = await CheckProductsQuantities(cart.CartProducts);
                    if (cartModelState.ErrorCount > 0)
                        return BadRequest(new
                        {
                            Success = false,
                            cartModelState
                        });
                }

                await _unitOfWork.CartRepository.AddAsync(cart);
                if (await _unitOfWork.Complete())
                {
                    var cartDTO = _mapper.Map<Cart, CartDTO>(cart);
                    return Ok(new
                    {
                        Success = true,
                        cart = cartDTO
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
        public async Task<ActionResult<CartDTO>> Update([FromQuery] string id, [FromBody] CartModel cartModel)
        {
            if (ModelState.IsValid)
            {
                var cart = _mapper.Map<CartModel, Cart>(cartModel);
                var cartExist = await _unitOfWork.CartRepository.GetById(id);
                if (cartExist == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Cart doesn't exist."
                    });

                if (cart.CartProducts?.Count > 0)
                {
                    var cartModelState = await CheckProductsQuantities(cart.CartProducts);
                    if (cartModelState.ErrorCount > 0)
                        return BadRequest(new
                        {
                            Success = false,
                            cartModelState
                        });
                }
                cartExist.CartProducts = cart.CartProducts;
                _unitOfWork.CartRepository.Update(cartExist);

                if (await _unitOfWork.Complete())
                    return Ok(new
                    {
                        Success = true,
                        cartExist
                    });
            }
            return BadRequest(new
            {
                Success = false,
                Message = "failed to update"
            });
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            _unitOfWork.CartRepository.Delete(id);

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
