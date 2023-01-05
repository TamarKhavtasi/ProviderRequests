using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace Idram.Api;

public interface IVendorService
{
    // ReSharper disable once InconsistentNaming
    public Task<VendorResponse> TransferToVendorAsync(TransferRequest request);
}

public class VendorService : IVendorService
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<VendorService> _logger;



    public VendorService(ILogger<VendorService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<VendorResponse> TransferToVendorAsync(TransferRequest request)
    {
        var message = BuildRequest(request);

        _logger.LogInformation("Request to Vendor:\n {RequestUri}", message.RequestUri);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(message);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("request to provider timed out ");
            return new VendorResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while sending vendor request {Method} {Url}", message.Method, message.RequestUri);
            return new VendorResponse();
        }

            return await ProcessResponseAsync(message, response);
    }

    private async Task<VendorResponse> ProcessResponseAsync(HttpRequestMessage message, HttpResponseMessage response)
    {
        TransferResponse assignmentResult;

        var responseToXml = new XmlSerializer(typeof(TransferResponse));
        try
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            assignmentResult = (TransferResponse)responseToXml.Deserialize(new StringReader(responseContent));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deserialize vendor XML");
            return new VendorResponse();
        }

        var vendorStatusCode = assignmentResult.Code.Trim();
        switch (vendorStatusCode)
        {
            case VendorStatusCodes.Balance:
            case VendorStatusCodes.AccessDenied:
            case VendorStatusCodes.DestAccount:
            case VendorStatusCodes.InternalError:
            case VendorStatusCodes.InvalidParameter:
            case VendorStatusCodes.InvalidSource:
            case VendorStatusCodes.ErrCode1:
            case VendorStatusCodes.ErrCode2:
            case VendorStatusCodes.ErrCode3:
            case VendorStatusCodes.ErrCode4:
            case VendorStatusCodes.ErrInvalidParam:
                return new VendorResponse                {
                    IsSuccess = false,
                    VendorStatus = vendorStatusCode
                };
        }

        return  new VendorResponse
        {
            IsSuccess = true,
            TransactionId = assignmentResult.Code
        };
    }

    private HttpRequestMessage BuildRequest(TransferRequest request)
    {
        var url = "https://test.idt.am/web/transinternal_as.aspx";
        var secret = "a6a7ab435d804a6891920fc21c23d0bf";
        var list = new List<string>
            {
                request.SourceAccount,
                request.Amount.ToString(),
                secret,
                request.DestAccount,
                request.SourceNumber,
                request.Request.ToString(),
                ((int)request.VendorDocumentType).ToString(),
                request.DocumentNumber
            };
        request.Checksum = list.ComputeChecksum();

        var keyValues = new Dictionary<string, string>
            {
                { "EDP_SOURCE_ACCOUNT", request.SourceAccount },
                { "EDP_DEST_ACCOUNT", request.DestAccount },
                { "EDP_AMOUNT", request.Amount.ToString() },
                { "EDP_SOURCE_NO", request.SourceNumber },
                { "EDP_REQUEST", request.Request.ToString() },
                { "EDP_CHECKSUM", request.Checksum },
                { "EDP_DOC_TYPE", request.VendorDocumentType.ToString("D") },
                { "EDP_DOC_NUMBER", request.DocumentNumber },
                { "EDP_TEST", request.EdpTest.ToString() }
            };
        return new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(url, keyValues));
    }
}