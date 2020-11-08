using Newtonsoft.Json;
using RestSharp;

namespace IntegrationCheckoutListsCleaner
{
    public class AuthRequest
    {
        public string AuthEndpoint { get; } = "https://integration-rest.fastrent.com/api/v4/auth/backend";
        public string Token { get; private set; }

        public string GetToken()
        {
            RestClient Client = new RestClient(AuthEndpoint);
            RestRequest Request = new RestRequest("request/oauth") { Method = Method.POST };
            Request.AddHeader("Accept", "application/json");
            Request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            Request.AddParameter("client_id", Config.Client_id);
            Request.AddParameter("client_secret", Config.Client_secret);
            Request.AddParameter("username", Config.BackendUsername);
            Request.AddParameter("password", Config.BackendPassword);
            Request.AddParameter("grant_type", "client_credentials");
            IRestResponse Response = Client.Execute(Request);
            return JsonConvert.DeserializeObject<dynamic>(Response.Content)["access_token"];
        }
    }
}