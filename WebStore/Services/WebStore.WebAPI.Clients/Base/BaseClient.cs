using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient HttpClient { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string address)
        {
            HttpClient = client;
            Address = address;
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>()
                .ConfigureAwait(false);
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PutAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.DeleteAsync(url).ConfigureAwait(false);
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed) return;
            _Disposed = true;

            if (disposing)
            {
                // должны освободить управляемые ресурсы
                //HttpClient.Dispose(); //не мы создавали, не нам и освобождать
            }

            // должны освободить неуправляемые ресурсы
        }
    }
}
