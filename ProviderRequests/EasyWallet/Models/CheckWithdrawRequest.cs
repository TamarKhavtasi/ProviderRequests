using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderRequests.EasyWallet.Models
{
    public class CheckWithdrawRequest
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string SessionID { get; set; }
        public bool NeedReceipt { get; set; }
    }
}
