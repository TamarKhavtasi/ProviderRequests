namespace ProviderRequests.EasyWallet.Models
{
    public class CheckWithdrawResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string PaymentSystemID { get; set; }
        public string Receipt { get; set; }
    }
}
