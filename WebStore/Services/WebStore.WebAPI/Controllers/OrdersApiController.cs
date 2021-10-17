using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]    
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
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _OrderService.GetOrderById(id);
            if (order is null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName)
        {
            var order = await _OrderService.CreateOrder(userName);
            return Ok(order);
        }
    }
}
