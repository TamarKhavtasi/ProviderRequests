using System.Runtime.Serialization;
using System.Xml.Linq;
namespace Idram.Api;

public class TransferRequest
{
    [DataMember(Name = "EDP_SOURCE_ACCOUNT")]
    public string SourceAccount { get; set; }

    [DataMember(Name = "EDP_DEST_ACCOUNT")]
    public string DestAccount { get; set; }

    [DataMember(Name = "EDP_AMOUNT")]
    public decimal Amount { get; set; }

    [DataMember(Name = "EDP_SOURCE_NO")]
    public string SourceNumber { get; set; }

    [DataMember(Name = "EDP_REQUEST")]
    public ulong Request { get; set; }

    [DataMember(Name = "EDP_CHECKSUM")]
    public string Checksum { get; set; }

    [DataMember(Name = "EDP_DOC_TYPE")]
    public VendorDocumentTypes VendorDocumentType { get; set; }

    [DataMember(Name = "EDP_DOC_NUMBER")]
    public string DocumentNumber { get; set; }

    [DataMember(Name = "EDP_TEST")]
    public int EdpTest { get; set; }
}
public enum VendorDocumentTypes
{
    Passport = 1,

    Ssn = 8,

    IdCard = 11
}