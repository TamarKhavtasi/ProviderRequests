using System.Collections.Generic;

namespace ProviderRequests.EasyWallet.Models
{
    public class ValidatePaymentAccountRequest
    {
        public string CheckSum { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ProviderID { get; set; }
        public Language LanguageID { get; set; }
        public string SessionID { get; set; }
        public List<string> Inputs { get; set; }
    }

    public enum Language
    {
        Armenian,
        English,
        Russian
    }
}
