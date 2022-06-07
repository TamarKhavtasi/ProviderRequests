namespace ProviderRequests.EasyWallet.Models
{
    public class CheckDepositRequest
    {
        public string CheckSum { get; set; }
        public string MerchantOrderId { get; set; }
        public string MerchantId { get; set; }
    }

    public enum VendorTransactionStatuses
    {
        Paid,
        Rejected,
        Pending,
        New
    }
}
