using NUnit.Framework;
using System.Collections.Generic;

namespace IntegrationCheckoutListsCleaner
{
    public class CheckoutListsCleaner
    {
        public string Token { get; private set; }
        public List<int> ContractToCancelId { get; private set; } = new List<int>();

        [OneTimeSetUp]
        public void GetAuthToken()
        {
            AuthRequest AuthRequest = new AuthRequest();
            Token = AuthRequest.GetToken();
        }

        [Test]
        public void CleanAllCheckoutLists()
        {
            GetContractsToCancel();
            CancelContracts();
        }

        private void GetContractsToCancel()
        {
            CheckoutListRequest CheckoutListRequest = new CheckoutListRequest(Token);
            ContractToCancelId = CheckoutListRequest.GetContractsToCancel();
        }

        private void CancelContracts()
        {
            ContractDeleteRequest ContractDeleteRequest = new ContractDeleteRequest(Token, ContractToCancelId);
            ContractDeleteRequest.CancelContracts();
        }
    }
}