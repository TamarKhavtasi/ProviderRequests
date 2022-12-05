using System;
using System.ServiceModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ProviderRequests.Ufc
{
    [MessageContract(WrapperName = "getTranIdResponse", WrapperNamespace = "http://ws.ecomm.ufc.ge/", IsWrapped = true)]
    public class GetTranIdResponse
    {
        [MessageBodyMember(Namespace = "http://ws.ecomm.ufc.ge/", Order = 0)]
        [XmlElement(Form = XmlSchemaForm.Unqualified, ElementName = "response")]
        public UfcSoapResponse Response { get; set; } = new UfcSoapResponse();

        [MessageBodyMember(Namespace = "http://ws.ecomm.ufc.ge/", Order = 1)]
        [XmlElement(Form = XmlSchemaForm.Unqualified, ElementName = "getTranIdResult")]
        public GetTranIdResult GetTranIdResult { get; set; } = new GetTranIdResult();
    }

    [Serializable]
    [XmlType(Namespace = "http://ws.ecomm.ufc.ge/")]
    public class UfcSoapResponse
    {
        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0, ElementName = "requestId")]
        public string RequestId { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1, ElementName = "responseCode")]
        public int ResponseCode { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2, ElementName = "responseMessage")]
        public string ResponseMessage { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://ws.ecomm.ufc.ge/")]
    public class GetTranIdResult
    {
        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0, ElementName = "ebppResult")]
        public EbppInResult EbppResult { get; set; } = new EbppInResult();
    }

    [Serializable]
    [XmlType(Namespace = "http://ws.ecomm.ufc.ge/")]
    public class EbppInResult
    {
        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0, ElementName = "provider")]
        public string Provider { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1, ElementName = "error")]
        public string Error { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2, ElementName = "description")]
        public string Description { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 3, ElementName = "paymentID")]
        public string PaymentId { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 4, ElementName = "paymentRef")]
        public string PaymentRef { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 5, ElementName = "authActionCode")]
        public string AuthActionCode { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 6, ElementName = "authApprCode")]
        public string AuthApprCode { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 7, ElementName = "authRefNumber")]
        public string AuthRefNumber { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 8, ElementName = "cardMask")]
        public string CardMask { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 9, ElementName = "transactions")]
        public TranIdResultEbppResultTransactions Transactions { get; set; } = new TranIdResultEbppResultTransactions { TranId = "123" };

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 10, ElementName = "status")]
        public string Status { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://ws.ecomm.ufc.ge/")]
    public class TranIdResultEbppResultTransactions
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0, ElementName = "tranId")]
        public string TranId { get; set; }
    }
}
