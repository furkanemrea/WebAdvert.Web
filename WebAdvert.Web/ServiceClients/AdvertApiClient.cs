using AdvertApi.Models;
using AdvertApi.Models.Response;
using Amazon.Runtime.Internal.Auth;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebAdvert.Web.ServiceClients.Abstraction;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = string.Empty;
        public AdvertApiClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            var createUrl = _configuration.GetSection("AdvertApi").GetValue<string>("CreateURL");

            _baseUrl = _configuration.GetSection("AdvertApi").GetValue<string>("BaseURL");

            _httpClient.BaseAddress = new Uri(createUrl);

            //_httpClient.DefaultRequestHeaders.Add(name: "Content-Type", value: "application/json");
        }

        public async Task<CreateAdvertResponse> Create(AdvertModel advertModel)
        {

            var content = new StringContent(JsonConvert.SerializeObject(advertModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(new Uri(uriString: $"{_baseUrl}/create"), content);

            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);

            return createAdvertResponse;
        }

        public async Task<bool> Confirm(ConfirmAdvertModel confirmAdvertModel)
        {
            var content = new StringContent(JsonConvert.SerializeObject(confirmAdvertModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(new Uri(uriString: $"{_baseUrl}/confirm"), content).ConfigureAwait(false);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

    }
}
