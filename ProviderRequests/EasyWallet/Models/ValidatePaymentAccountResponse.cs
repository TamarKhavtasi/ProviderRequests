using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderRequests.EasyWallet.Models
{
    public class ValidatePaymentAccountResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string Debt { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal MinAmount { get; set; }
        public string SubscriberID { get; set; }
        public List<Property> Properties { get; set; }

        public List<Result> MultipleResult { get; set; }
    }

    public class Result
    {
        public string Amount { get; set; }
        public string BalanceResponse { get; set; }
        public List<string> Inputs { get; set; }
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
