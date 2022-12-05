using System;
using System.Collections.Generic;

namespace ProviderRequests.EasyWallet.Models
{
    public class WithdrawResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string PaymentSystemID { get; set; }
        public int? Receipt { get; set; }
    }
}
