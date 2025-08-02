using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignatureIntegration.Connector
{
    internal class ConnectorForV6: IConnectorForV6
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public ConnectorForV6() 
        {}

        public async Task<JObject> PostAsync(Uri endpoint, JObject body, NetworkCredential credentials = null, string token = "")
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                string jsonBody = body.ToString();

                request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else if (credentials != null)
                {
                    string base64 = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{credentials.UserName}:{credentials.Password}"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
                }

                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    return jsonResponse;
                }
            }
        }
    }
}
