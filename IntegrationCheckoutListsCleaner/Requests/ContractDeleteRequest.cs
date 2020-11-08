using RestSharp;
using System;
using System.Collections.Generic;

namespace IntegrationCheckoutListsCleaner
{
    public class ContractDeleteRequest
    {
        public string CancelContractEndpoint { get; } = "https://integration-rest.fastrent.com/api/v4/contract";
        public string Token { get; private set; }
        public List<int> ContractToCancelId { get; private set; } = new List<int>();

        public ContractDeleteRequest(string Token, List<int> ContractToCancelId)
        {
            this.Token = Token;
            this.ContractToCancelId = ContractToCancelId;
        }

        public void CancelContracts()
        {
            if (ContractToCancelId.Count > 0)
            {
                foreach (int ContractId in ContractToCancelId)
                {
                    SendRequest(ContractId);
                    Console.WriteLine(string.Format($"Cancelled contract with id {ContractId}"));
                }
            }
            else
            {
                Console.WriteLine("No contract has been cancelled");
            }
        }

        private void SendRequest(int ContractId)
        {
            RestClient Client = new RestClient(CancelContractEndpoint);
            RestRequest Request = new RestRequest(Method.DELETE);
            Request.AddParameter("Authorization", "Bearer " + Token, ParameterType.HttpHeader);
            Request.AddParameter("lang", "en");
            Request.AddParameter("contract_id", ContractId);
            Request.AddParameter("avoid_cancellation_fee", "true");
            Request.AddParameter("no_credit", "true");
            IRestResponse Response = Client.Execute(Request);
        }
    }
}