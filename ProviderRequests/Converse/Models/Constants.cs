namespace ProviderRequests.Converse.Models
{
    public static class Constants
    {
        public const string Url = "https://ipaytest.arca.am:8445/payment/rest";
        public const string UserName = "adjarabet_test_api";
        public const string Password = "Test123";
        public const string RecUserName = "adjarabet_test_binding";
        public const string RecPassword = "Test123";
        public const string CardNumber = "5134571100000080";
        public const string CardCVV = "615";
        public const string CardExpiry = "12/2022";

    }

    public static class VendorEndpoints
    {
        /// <summary>
        /// register is used for registering deposit transactions in the provider’s system.
        /// </summary>
        public const string Register = "register.do?";
        /// <summary>
        /// paymentOrderBinding method is used to bind the payment accounts to the user.
        /// </summary>
        public const string Binding = "paymentOrderBinding.do";
        /// <summary>
        /// reverse method is used in order to cancel the payment and roll back it in the provider's system.
        /// </summary>
        public const string Reverse = "reverse.do";
        /// <summary>
        /// getOrderStatusExtended method is used in order to check the transaction’s status.
        /// </summary>
        public const string GetOrderStatusExtended = "getOrderStatusExtended.do";
    }
}
