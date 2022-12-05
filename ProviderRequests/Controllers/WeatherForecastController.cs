using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProviderRequests.EasyWallet;
using ProviderRequests.EasyWallet.Models;
using ProviderRequests.Ufc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ProviderRequests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IVendorService _vendor;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IVendorService vendor)
        {
            _logger = logger;
            _vendor = vendor;
        }

        [HttpGet]
        public async Task Get([FromQuery] TestModel testModel)
        {

            var phone = "995597750080";
            var result = phone.StartsWith("995") ? phone.Substring(3, 9) : phone;
            var te = "";
            //var depositResponse = await _vendor.DepositAsync(new DepositRequest
            //{
            //    Amount = 1200,
            //    MerchantOrderId = "1655001",
            //    MerchantId = Constants.MerchantId
            //});

            var depositCheckResponse = await _vendor.CheckDepositStatusAsync(new CheckDepositRequest
            {
                MerchantOrderId = "1262011",
                MerchantId = Constants.MerchantId,
                Guid= "ff4d4ac4-a1c2-4250-bdf4-126a48e58c52"
            });

            //var add = new AccountAdditionalData().ToJson();

            //var yy = AdditionalData.FromJson<AccountAdditionalData>(add);

            //var xz = new GetTranIdResponse();
            //string xml = GetXMLFromObject(xz);
            //var sessionID = "e82e7669-7702-45b9-bf68-1d32a6fa885a";
            //var withdrawCheckResponse = await _vendor.CheckWithdrawStatusAsync(new CheckWithdrawRequest
            //{
            //    Password = Constants.Password,
            //    UserName = Constants.Username,
            //    NeedReceipt = false,
            //    SessionID = sessionID
            //});
            //var validateAccountRespose = await _vendor.ValidatePaymentAccountAsync(new ValidatePaymentAccountRequest
            //{
            //    InputValues = new List<string> { "+37477605588" },
            //    Language = (int)Language.English,
            //    Password = Constants.Password,
            //    UserName = Constants.Username,
            //    ProviderId = Constants.ProviderId,
            //    SessionID = sessionID
            //});

            //var withdrawResponse = await _vendor.WithdrawAsync(
            //     new WithdrawRequest
            //     {
            //         Password = Constants.Password,
            //         UserName = Constants.Username,
            //         Amount = 1200,
            //         Commission = 0,
            //         CurrencyISO = "AMD",
            //         Inputs = new List<string> { "+37477605588" },
            //         ProviderID = Constants.ProviderId,
            //         SessionID = sessionID,
            //         SystemTime = DateTime.Now,
            //         NeedReceipt = false
            //     });

        }

        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }
    }

    public class AccountAdditionalData : AdditionalData
    {
        public string LastUsedDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        public DateTime LastUsedDate1 { get; set; } = DateTime.Now;
    }

    public abstract class AdditionalData
    {
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TAdditionalData FromJson<TAdditionalData>(string json) where TAdditionalData : AdditionalData
        {
            return JsonConvert.DeserializeObject<TAdditionalData>(json);
        }
    }
}
