using System.Net.Http;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient HttpClient { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string address)
        {
            HttpClient = client;
            Address = address;
        }
    }
}
