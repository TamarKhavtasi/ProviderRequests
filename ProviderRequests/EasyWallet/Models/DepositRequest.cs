using Newtonsoft.Json;

namespace ProviderRequests.EasyWallet.Models
{
    public class DepositRequest
    {
        [JsonProperty("MerchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("MerchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [JsonProperty("Checksum")]
        public string CheckSum { get; set; }
    }
}
