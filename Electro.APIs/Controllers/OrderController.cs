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
    public class OrderController : APIBaseController
    {
        private readonly UnitOfWork<Order> _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(UnitOfWork<Order> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private async Task<ModelStateDictionary> CheckProductsQuantities(ICollection<OrderProduct> orderProducts)
        {
            foreach (var item in orderProducts)
            {
                var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                if (item.Quantity > product.QuantityInStock)
                    ModelState.AddModelError($"Product{item.ProductId}", "can't have quantity more than that in stock");
            }
            return ModelState;
        }

        [MyAuthorizer("Admin")]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAll();
            var ordersDTO = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
            return Ok(new
            {
                Success = true,
                orders = ordersDTO
            });
        }

        [HttpGet]
        public async Task<ActionResult<OrderDTO?>> GetOrder([FromQuery] string id, [FromQuery] DateTime date)
        {
            var order = await _unitOfWork.OrderRepository.GetOrder(id, date);
            if (order == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Order Not found."
                });
            var orderDTO = _mapper.Map<Order, OrderDTO>(order);
            return Ok(new
            {
                Success = true,
                order = orderDTO
            });
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Add([FromBody] OrderModel orderModel)
        {
            var order = _mapper.Map<OrderModel, Order>(orderModel);
            if (order.OrderProducts?.Count > 0)
            {
                var orderModelState = await CheckProductsQuantities(order.OrderProducts);
                if (orderModelState.ErrorCount > 0)
                    return BadRequest(new
                    {
                        Success = false,
                        orderModelState
                    });
            }

            await _unitOfWork.OrderRepository.AddAsync(order);
            if (await _unitOfWork.Complete())
            {
                var orderDTO = _mapper.Map<Order, OrderDTO>(order);
                return Ok(new
                {
                    Success = true,
                    order = orderDTO
                });
            }
            return BadRequest(new
            {
                Success = false,
                Message = "failed to add"
            });
        }

        //[HttpPut]
        //public async Task<ActionResult<Order>> Update([FromQuery] string id, [FromBody] Order order)
        //{
        //    var OrderExist = await UnitOfWork.OrderRepository.GetById(id);
        //    if (OrderExist == null)
        //        return NotFound("Order doesn't exist.");

        //    if (order.OrderProducts?.Count > 0)
        //    {
        //        var OrderModelState = await CheckProductsQuantities(order.OrderProducts);
        //        if (OrderModelState.ErrorCount > 0)
        //            return BadRequest(OrderModelState);
        //    }
        //    OrderExist.OrderProducts = order.OrderProducts;
        //    UnitOfWork.OrderRepository.Update(OrderExist);

        //    if (await UnitOfWork.Complete())
        //        return Ok(OrderExist);
        //    return BadRequest("failed to update");
        //}

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string id, DateTime date)
        {
            await _unitOfWork.OrderRepository.Delete(id, date);

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
