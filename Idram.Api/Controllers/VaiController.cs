using Microsoft.AspNetCore.Mvc;

namespace Idram.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VaiController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        public VaiController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet(Name = "test")]
        public async Task<IActionResult> TransferAsync()
        {
            var request = new TransferRequest
            {
                SourceAccount = "110001905",
                DestAccount = "556759621",
                Amount = 100,
                SourceNumber = "CB39FBD823EB2BD9A9A129931E30F9AF",
                Request = 1276436,
                DocumentNumber = "3424542346498",
                VendorDocumentType = VendorDocumentTypes.Ssn,
                EdpTest = 0
            };

            var response = await _vendorService.TransferToVendorAsync(request);

            return Ok();
        }
    }
}