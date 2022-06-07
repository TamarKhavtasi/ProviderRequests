using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProviderRequests.EasyWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProviderRequests.EasyWallet
{
    public interface IVendorService
    {
        Task<string> DepositAsync(DepositRequest request);

        Task<string> CheckDepositStatusAsync(CheckDepositRequest request);

        //ValidatePaymentAccountResponse ამას უნდა აბრუნებდეს მაგრამ ვნახოთ რას დააბრუნებს და მერე გავასწოროთ
        Task<string> ValidatePaymentAccountAsync(ValidatePaymentAccountRequest request);

        Task<string> WithdrawAsync(WithdrawRequest request);

        //CheckWithdrawResponse
        Task<string> CheckWithdrawStatusAsync(CheckWithdrawRequest request);
    }

    public class VendorService : IVendorService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<VendorService> _logger;

        public VendorService(IHttpClientFactory httpClientFactory, ILogger<VendorService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> DepositAsync(DepositRequest request)
        {
            request.CheckSum = ComputeSignature(Constants.DepositToken, Constants.MerchantId.ToString(), request.MerchantOrderId, request.Amount.ToString());

            return await DoRequestAsync<string>(request, Constants.DepositUrl, "createorder");
        }

        public async Task<string> CheckDepositStatusAsync(CheckDepositRequest request)
        {
            request.CheckSum = ComputeSignature(Constants.DepositToken, Constants.MerchantId.ToString(), request.MerchantOrderId);

            return await DoRequestAsync<string>(request, Constants.DepositUrl, "checkorder");

            //ნუ სანახავია რას და როგორ აბრუნებს სატესტოდ დავწერ
        }

        public async Task<string> ValidatePaymentAccountAsync(ValidatePaymentAccountRequest request)
        {
            request.CheckSum = ComputeSignature(Constants.WithdrawToken, request.ProviderID, request.Inputs[0], request.Inputs[1]);

            return await DoRequestAsync<string>(request, Constants.WithdrawUrl, "check");
        }

        public async Task<string> WithdrawAsync(WithdrawRequest request)
        {
            request.CheckSum = ComputeSignature(request.ProviderID, request.SystemTime, request.Inputs[1], request.Amount);

            return await DoRequestAsync<string>(request, Constants.WithdrawUrl, "pay");
        }

        public async Task<string> CheckWithdrawStatusAsync(CheckWithdrawRequest request)
        {
            return await DoRequestAsync<string>(request, Constants.WithdrawUrl, "getpaymentinfo");
        }

        private async Task<string> DoRequestAsync<TResponse>(object requestModel, string url, string method)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var message = new HttpRequestMessage(HttpMethod.Post, $"{url}/{method}");
            var requestBody = JsonConvert.SerializeObject(requestModel, Formatting.None);
            message.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await httpClient.SendAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while sending vendor request {Method} {Url}", message.Method, message.RequestUri);
                return "Vendor request failed";
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            //_logger.LogVendorHttpResponse(message, response, responseContent);

            if (!response.IsSuccessStatusCode)
                return "provider error";
            //return new OperationResult<TResponse>(ErrorCodes.GenericFailed, "Got error from provider");

            var responseModel = "";
            try
            {
                responseModel = JsonConvert.DeserializeObject<string>(responseContent);
            }
            catch (Exception e)
            {
                const string errorMessage = "Can't deserialize response from vendor";
                _logger.LogError(e, errorMessage);
                //return new OperationResult<TResponse>(ErrorCodes.GenericFailed, errorMessage);
            }

            return responseModel;
        }

        private string ComputeSignature(string secret, params object[] fields)
        {
            var input = string.Join("", fields) + secret;
            var result = "";
            using (MD5 md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                result = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
            return result;
        }
    }
}
