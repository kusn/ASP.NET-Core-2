using Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebStore.TestConsole
{
    class Program
    {        
        static async Task Main(string[] args)
        {
            var client = new HttpClient()
            {
                //BaseAddress = new Uri("http://localhost:5001")
            };

            var api = new WebAPIClient("http://localhost:5001", client);

            var products = api.ProductsAsync(new ProductFilter());

            var employee = await api.Employees4Async(2);

            Console.ReadKey();
        }
    }
}
