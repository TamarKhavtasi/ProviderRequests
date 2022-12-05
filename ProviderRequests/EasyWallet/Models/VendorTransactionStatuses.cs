using Singular.CorePlatform.Common.CustomTypes;

namespace ProviderRequests.EasyWallet.Models
{
    public class VendorTransactionStatuses : ValueType<VendorTransactionStatuses, string>

    {
        public static VendorTransactionStatuses Paid { get; set; } = "paid";
        public static VendorTransactionStatuses Rejected { get; set; } = "rejected";
        public static VendorTransactionStatuses Pending { get; set; } = "pending";
        public static VendorTransactionStatuses New { get; set; } = "new";

        public VendorTransactionStatuses(string value)
            : base(value)
        {
        }

        public static implicit operator VendorTransactionStatuses(string value) => new VendorTransactionStatuses(value);

        public static implicit operator string(VendorTransactionStatuses custom) => custom._value;
    }
}
