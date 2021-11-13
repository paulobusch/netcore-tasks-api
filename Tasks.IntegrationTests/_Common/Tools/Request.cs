using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain._Common.Enums;

namespace Tasks.IntegrationTests._Common.Tools
{
    public class Request
    {
        private readonly HttpClient _client;
        public Request(HttpClient client) => _client = client;

        public async Task<(Status status, TResult result)> GetAsync<TResult>(Uri uri, dynamic data = null) where TResult : class
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{uri}?{GetUrlString(data)}"),
                Method = HttpMethod.Get
            };

            try
            {
                var response = await _client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(json);
                return ((Status)response.StatusCode, result);
            }
            catch (Exception)
            {
                return (Status.Error, null);
            }
        }

        public async Task<(Status status, TResult result)> PostAsync<TResult>(Uri uri, dynamic data) where TResult : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.Default, "application/json");

            try
            {
                var response = await _client.PostAsync(uri, content);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(json);
                return ((Status)response.StatusCode, result);
            }
            catch (Exception)
            {
                return (Status.Error, null);
            }
        }

        public async Task<(Status status, TResult result)> PutAsync<TResult>(Uri uri, dynamic data) where TResult : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.Default, "application/json");

            try
            {
                var response = await _client.PutAsync(uri, content);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(json);
                return ((Status)response.StatusCode, result);
            }
            catch (Exception)
            {
                return (Status.Error, null);
            }
        }

        public async Task<(Status status, TResult result)> DeleteAsync<TResult>(Uri uri) where TResult : class
        {
            try
            {
                var response = await _client.DeleteAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(json);
                return ((Status)response.StatusCode, result);
            }
            catch (Exception)
            {
                return (Status.Error, null);
            }
        }

        private string GetUrlString(object data = null)
        {
            if (data == null) return string.Empty;
            var props = data
                .GetType().GetProperties()
                .Where(p => p.GetValue(data) != null)
                .Select(p => $"{p.Name}={JsonConvert.SerializeObject(p.GetValue(data)).Replace(@"""", "")}");
            return string.Join("&", props);
        }
    }
}
