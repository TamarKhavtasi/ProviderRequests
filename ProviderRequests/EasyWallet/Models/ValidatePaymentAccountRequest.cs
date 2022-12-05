using System.Collections.Generic;

namespace ProviderRequests.EasyWallet.Models
{
    public class ValidatePaymentAccountRequest
    {
        public string CheckSum { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ProviderId { get; set; }
        public int Language { get; set; }
        public string SessionID { get; set; }
        public List<string> InputValues { get; set; }
    }

    public enum Language
    {
        Armenian,
        English,
        Russian
    }
}
