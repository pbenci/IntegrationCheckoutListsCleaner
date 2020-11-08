using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace IntegrationCheckoutListsCleaner
{
    public class CheckoutListRequest
    {
        public string Token { get; private set; }
        public string CheckoutListEndpoint { get; } = "https://integration-rest.fastrent.com/api/v4/checkout/list";
        public List<string> BranchesSlugs { get; } = new List<string> { "castione", "affoltern", "geneva", "test", "villeneuve" };
        public List<int> ContractToCancelId { get; private set; } = new List<int>();
        public IRestResponse Response { get; private set; }
        public int TotalNumberOfContractsToCancel { get; private set; }
        public dynamic DeserializedJson { get; private set; }

        public CheckoutListRequest(string Token)
        {
            this.Token = Token;
        }

        public List<int> GetContractsToCancel()
        {
            foreach (string BranchSlug in BranchesSlugs)
            {
                SendRequest(BranchSlug);
                DeserializeJson();
                DeserializeTotalContractsNumber();
                AddContractsToTheListOfContractsThatMustBeCancelled();
            }
            return ContractToCancelId;
        }

        private void SendRequest(string BranchSlug)
        {
            RestClient Client = new RestClient(CheckoutListEndpoint);
            RestRequest Request = new RestRequest(Method.GET);
            Request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
            Request.AddParameter("lang", "en");
            Request.AddParameter("branch_slug", BranchSlug);
            Response = Client.Execute(Request);
            DeserializedJson = Response.Content;
        }

        private void DeserializeJson()
        {
            DeserializedJson = JsonConvert.DeserializeObject<dynamic>(Response.Content);
        }

        private void DeserializeTotalContractsNumber()
        {
            TotalNumberOfContractsToCancel = DeserializedJson["content"]["total_contracts"];
        }

        private void AddContractsToTheListOfContractsThatMustBeCancelled()
        {
            for (int i = 0; i < TotalNumberOfContractsToCancel; i++)
            {
                ContractToCancelId.Add(Convert.ToInt32(DeserializedJson["content"]["checkout_list"][i]["contract"]["id"]));
            }
        }
    }
}