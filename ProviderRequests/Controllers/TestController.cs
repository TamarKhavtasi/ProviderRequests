using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProviderRequests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {


        [HttpGet]
        public async Task GetAsync([FromQuery] RequestModel testModel)
        {


            var request = new GetTransactionsListRequest(DateTime.Now,
                DateTime.UtcNow,
                2,
                TransactionType.ProviderToCore,
                new TransactionStatus[] { TransactionStatus.AmountWithdrawnFromCoreAccount, TransactionStatus.Approved, TransactionStatus.Pending },
                new Guid(),
                12,
                122,
                new Guid(),
                "test",
                12,
                121222
                , "");
            QueryBuilder keyValues = new QueryBuilder();
            var valuePairs = request?.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(request) ?? "");
            var transactionStatuses = "";
            foreach (var item in valuePairs)
            {
                if (item.Key == "StatusArray" && item.Value != null)
                    transactionStatuses = string.Join("&statusarray=", request.StatusArray.Cast<int>());
                else if (item.Key == "FromDate" || item.Key == "ToDate")
                    keyValues.Add(item.Key, (Convert.ToDateTime(item.Value)).ToString("yyyy-MM-dd"));
                else keyValues.Add(item.Key, item.Value?.ToString());
            }
            var result = keyValues + transactionStatuses;

            testModel = new RequestModel
            {
                Amount = 1,
                Id = 15,
                Name = "test",
                Statuses = new int[] { 12, 12, 12, 3243 }
            };

            QueryBuilder keyValues1 = new QueryBuilder();

            keyValues.Add(nameof(testModel.Id), testModel.Id.ToString());
            keyValues.Add(nameof(testModel.Name), testModel.Name);
            keyValues.Add(nameof(testModel.Amount), testModel.Amount.ToString());
            keyValues.Add(nameof(testModel.Statuses), String.Join(',', testModel.Statuses));

            var xz = QueryBuilder<RequestModel>(testModel, "");

            var test = xz;
        }
        private string QueryBuilder<TRequest>(TRequest request, string path)
        {

            var test = request?.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(request) ?? "");

            var result = QueryHelpers.AddQueryString(path, (IDictionary<string, string?>)test);

            return result;
        }
    }

    public record GetTransactionsListRequest(
    [property: JsonPropertyName("fromDate")] DateTime FromDate,
    [property: JsonPropertyName("toDate")] DateTime ToDate,
    [property: JsonPropertyName("id")] uint? Id,
    [property: JsonPropertyName("transactionType")]
    [property: JsonConverter(typeof(JsonStringEnumConverter))] TransactionType? TransactionType,
    [property: JsonPropertyName("statusArray")] TransactionStatus[] StatusArray,
    [property: JsonPropertyName("transactionId")] Guid? TransactionId,
    [property: JsonPropertyName("take")] int Take,
    [property: JsonPropertyName("skip")] int Skip,
    Guid? ProviderId,
    string? ProviderLabel,
    uint? ServiceId,
    uint? UserId,
    string? ProviderTransactionId) : BaseTransactionRequest(ServiceId, ProviderId, ProviderLabel, UserId, ProviderTransactionId);

    public record BaseTransactionRequest(
    [property: JsonPropertyName("serviceId")] uint? ServiceId,
    [property: JsonPropertyName("providerId")] Guid? ProviderId,
    [property: JsonPropertyName("providerLabel")] string? ProviderLabel,
    [property: JsonPropertyName("userId")] uint? UserId,
    [property: JsonPropertyName("providerTransactionId")] string? ProviderTransactionId
);
    public enum TransactionStatus
    {
        Initialize = 0,
        Started = 1,
        ReceivedQueue = 2,
        ReceivedRequestFromProvider = 3,
        Canceled = 6,
        Failed = 20, // 0x00000014
        FailedFromProvider = 21, // 0x00000015
        Rejected = 22, // 0x00000016
        FailedAmountLimitExceeded = 25, // 0x00000019
        FailedAccountLimitExceeded = 26, // 0x0000001A
        AmountWithdrawnFromProviderAccount = 30, // 0x0000001E
        AmountWithdrawnFromCoreAccount = 31, // 0x0000001F
        StartRollback = 49, // 0x00000031
        Rollback = 50, // 0x00000032
        RollbackFailed = 51, // 0x00000033
        Locked = 97,
        ProviderPendingTransaction = 98, // 0x00000062
        Pending = 99, // 0x00000063
        Finished = 100, // 0x00000064
        Approved = 101, // 0x00000065
        Duplicated = 110 // 0x0000006E
    }

    public enum TransactionType
    {
        CoreToProvider = 1,
        ProviderToCore = 2,
        AccountRegistration = 3
    }
    public class RequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public int[] Statuses { get; set; }
    }
}