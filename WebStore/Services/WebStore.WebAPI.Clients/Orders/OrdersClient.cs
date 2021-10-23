using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient client) : base(client, WebAPIAddresses.Orders)
        {

        }

        public async Task<Order> CreateOrder(string userName, CartViewModel Cart, OrderViewModel orderViewModel)
        {
            var model = new CreateOrderDTO
            {
                Items = Cart.ToDTO(),
                Order = orderViewModel,
            };

            var response = await PostAsync($"{Address}/{userName}", model).ConfigureAwait(false);
            var order = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<OrderDTO>()
                .ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{userName}").ConfigureAwait(false);
            return orders.FromDTO();
        }
    }
}
