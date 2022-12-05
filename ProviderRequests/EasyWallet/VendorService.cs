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
        Task<ValidatePaymentAccountResponse> ValidatePaymentAccountAsync(ValidatePaymentAccountRequest request);

        Task<WithdrawResponse> WithdrawAsync(WithdrawRequest request);

        //CheckWithdrawResponse
        Task<List<WithdrawResponse>> CheckWithdrawStatusAsync(CheckWithdrawRequest request);
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
            request.Checksum = ComputeSignature(Constants.DepositToken, Constants.MerchantId.ToString(), request.MerchantOrderId, request.Guid);
            return await DoRequestAsync<string>(request, Constants.DepositUrl, "checkorder");

            //ნუ სანახავია რას და როგორ აბრუნებს სატესტოდ დავწერ
        }

        public async Task<ValidatePaymentAccountResponse> ValidatePaymentAccountAsync(ValidatePaymentAccountRequest request)
        {
            request.CheckSum = ComputeSignature(Constants.WithdrawToken, request.ProviderId, request.InputValues[0]);

            return await DoRequestAsync<ValidatePaymentAccountResponse>(request, Constants.WithdrawUrl, "check");
        }

        public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequest request)
        {
            request.CheckSum = ComputeSignature(Constants.WithdrawToken, request.ProviderID, request.SystemTime.ToString("yyyyMMddHHmm"), request.Inputs[0], request.Amount);

            return await DoRequestAsync<WithdrawResponse>(request, Constants.WithdrawUrl, "pay");
        }

        public async Task<List<WithdrawResponse>> CheckWithdrawStatusAsync(CheckWithdrawRequest request)
        {
            return await DoRequestAsync<List<WithdrawResponse>>(request, Constants.WithdrawUrl, "getpaymentinfo");
        }

        private async Task<TResponse> DoRequestAsync<TResponse>(object requestModel, string url, string method)
        {
            using (var handler = new HttpClientHandler())
            {
                // allow the bad certificate
                handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
                using (var httpClient = new HttpClient(handler))
                {
                    //var httpClient = _httpClientFactory.CreateClient();
                    var message = new HttpRequestMessage(HttpMethod.Post, $"{url}/{method}");
                    var requestBody = JsonConvert.SerializeObject(requestModel, Formatting.None);
                    message.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response;

                    try
                    {
                        response = await httpClient.SendAsync(message);
                        var responseContent = await response.Content.ReadAsStringAsync();
                        TResponse responseModel;

                        try
                        {
                            responseModel = JsonConvert.DeserializeObject<TResponse>(responseContent);
                            return responseModel;
                        }
                        catch (Exception e)
                        {
                            const string errorMessage = "Can't deserialize response from vendor";
                            _logger.LogError(e, errorMessage);
                            //return new OperationResult<TResponse>(ErrorCodes.GenericFailed, errorMessage);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error while sending vendor request {Method} {Url}", message.Method, message.RequestUri);
                        //return new OperationResult<TResponse>(ErrorCodes.GenericFailed, "Vendor request failed");
                    }
                }
            }
            return default(TResponse);
            //HttpResponseMessage response;

            //var responseContent = await response.Content.ReadAsStringAsync();
            ////_logger.LogVendorHttpResponse(message, response, responseContent);

            //if (!response.IsSuccessStatusCode)
            //    return "provider error";
            ////return new OperationResult<TResponse>(ErrorCodes.GenericFailed, "Got error from provider");

            //var responseModel = "";
            //try
            //{
            //    responseModel = JsonConvert.DeserializeObject<string>(responseContent);
            //}
            //catch (Exception e)
            //{
            //    const string errorMessage = "Can't deserialize response from vendor";
            //    _logger.LogError(e, errorMessage);
            //    //return new OperationResult<TResponse>(ErrorCodes.GenericFailed, errorMessage);
            //}

            //return responseModel;
        }

        private string ComputeSignature(string secret, params object[] fields)
        {
            var input = string.Join("", fields) + secret;
            string result;
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                _logger.LogDebug("Computing Signature, base string: {Input}", input);
                result = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                _logger.LogDebug("Signature result: {Result}", result);
            }

            return result;
        }

    }
}
