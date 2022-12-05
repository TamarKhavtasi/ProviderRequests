using Newtonsoft.Json;

namespace ProviderRequests.EasyWallet.Models
{
    public class CheckDepositRequest
    {
        [JsonProperty("Checksum")]
        public string Checksum { get; set; }

        [JsonProperty("MerchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("MerchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("Guid")]
        public string Guid { get; set; }
    }

    //public enum VendorTransactionStatuses
    //{
    //    Paid,
    //    Rejected,
    //    Pending,
    //    New
    //}
}
