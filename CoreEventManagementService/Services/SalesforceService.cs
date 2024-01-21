using CoreEventManagementService.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CoreEventManagementService.Configurations;

namespace CoreEventManagementService.Services
{
    public class SalesforceService : ISalesforceService
    {
        IOptions<SalesforceConfigurations> _config;
        public SalesforceService(IOptions<SalesforceConfigurations> config)
        {
            _config = config;
        }
        public async Task<string> GetSalesforceAuthToken()
        {
            using (var _httpClient = new HttpClient())
            {
                var builder = new UriBuilder(_config.Value.LoginAuthTokenUrl);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["client_id"] = _config.Value.Client_id;
                query["client_secret"] = _config.Value.ClientSecret;
                query["username"] = _config.Value.Username;
                query["password"] = _config.Value.Password;
                query["grant_type"] = "password";
                builder.Query = query.ToString();
                var baseUrl = builder.ToString();

                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

                var responseMessage = _httpClient.PostAsync(baseUrl, new FormUrlEncodedContent(postData)).Result;

                var authResponse = await responseMessage.Content.ReadFromJsonAsync<SalesforceAuthResponse>().ConfigureAwait(false);

                return authResponse?.access_token;
            }
        }

        public void InsertGuestDetailsRecord(GuestDetails_sf guestDetailsRequest)
        {
            using (var _httpClient = new HttpClient())
            {
                string authToken = SalesforceTokenManager.GetToken();

                var responseMessage = PostGuestDetails(guestDetailsRequest, _httpClient, authToken);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    authToken = GetSalesforceAuthToken().Result;
                    SalesforceTokenManager.SetToken(authToken);
                    PostGuestDetails(guestDetailsRequest, _httpClient, authToken);
                }
            }
        }

        private HttpResponseMessage PostGuestDetails(GuestDetails_sf guestDetailsRequest, HttpClient _httpClient, string authToken)
        {
            var builder = new UriBuilder(_config.Value.GuestDetailsUrl);
            var baseUrl = builder.ToString();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpContent body = new StringContent(JsonSerializer.Serialize(guestDetailsRequest), Encoding.UTF8, "application/json");

            var responseMessage = _httpClient.PostAsync(baseUrl, body).Result;

            return responseMessage;
        }     
    }
}
