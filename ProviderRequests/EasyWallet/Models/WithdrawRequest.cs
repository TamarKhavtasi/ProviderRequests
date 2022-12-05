using System;
using System.Collections.Generic;

namespace ProviderRequests.EasyWallet.Models
{
    public class WithdrawRequest
    {
        public string CheckSum { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Amount { get; set; }
        public int Commission { get; set; }
        public string CurrencyISO { get; set; }
        public List<string> Inputs { get; set; }
        public string ProviderID { get; set; }
        public string RangeID { get; set; }
        public string SessionID { get; set; }
        public DateTime SystemTime { get; set; }
        public bool NeedReceipt { get; set; }
    }
}
