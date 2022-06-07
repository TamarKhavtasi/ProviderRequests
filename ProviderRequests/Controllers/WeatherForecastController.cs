using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProviderRequests.EasyWallet;
using ProviderRequests.EasyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task Get()
        {
            var depositResponse = await _vendor.DepositAsync(new DepositRequest
            {
                Amount = 1200,
                MerchantOrderId = "9383800",
                MerchantId = Constants.MerchantId
            });

            var depositCheckResponse = await _vendor.CheckDepositStatusAsync(new CheckDepositRequest
            {
                MerchantOrderId = "9383800",
                MerchantId = Constants.MerchantId
            });

            var sessionID = Guid.NewGuid().ToString();
            var validateAccountRespose = await _vendor.ValidatePaymentAccountAsync(new ValidatePaymentAccountRequest
            {
                Inputs = new List<string> { "+37477605588", "" },
                LanguageID = Language.Armenian,
                Password = Constants.Password,
                UserName = Constants.Username,
                ProviderID = Constants.ProviderId,
                SessionID = sessionID
            });

            var withdrawResponse = await _vendor.WithdrawAsync(
                 new WithdrawRequest
                 {
                     Password = Constants.Password,
                     UserName = Constants.Username,
                     Amount = 1200,
                     Commission = 0,
                     CurrencyISO = "AMD",
                     Inputs = new List<string> { "+37477605588", "" },
                     ProviderID = Constants.ProviderId,
                     SessionID = sessionID,
                     SystemTime = DateTime.Now.ToString("yyyyMMddHHmm"),
                     NeedReceipt = false
                 });

            var withdrawCheckResponse = await _vendor.CheckWithdrawStatusAsync(new CheckWithdrawRequest
            {
                Password = Constants.Password,
                UserName = Constants.Username,
                NeedReceipt = false,
                SessionID = sessionID
            });
        }
    }
}
