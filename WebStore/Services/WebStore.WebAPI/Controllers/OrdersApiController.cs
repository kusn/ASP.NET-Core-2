using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(WebAPIAdresses.Orders)]
    public class OrdersApiController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService orderService)
        {
            _OrderService = orderService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _OrderService.GetUserOrders(userName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _OrderService.GetOrderById(id);
            if (order is null)
                return NotFound();
            return Ok(order.ToDTO());
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderModel)
        {
            var order = await _OrderService.CreateOrder(userName, orderModel.Items.ToCartView(), orderModel.Order);
            return Ok(order.ToDTO());
        }
    }
}
