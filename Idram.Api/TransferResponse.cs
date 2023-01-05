using System.Xml.Serialization;

namespace Idram.Api;
[XmlRoot(ElementName = "response")]
public class TransferResponse
{
    [XmlElement(ElementName = "code")]
    public string Code { get; set; }

    [XmlElement(ElementName = "cust_name")]
    public string CustName { get; set; }
}