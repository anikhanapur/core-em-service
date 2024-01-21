using CoreEventManagementService.Models;
using CoreEventManagementService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreEventManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestDetailsController : ControllerBase
    {
        private ISalesforceService _salesforceService;
        public GuestDetailsController(ISalesforceService salesforceService)
        {
            _salesforceService = salesforceService;
        }

        [HttpPost(Name = "SaveGuestDetails")]
        public void SaveGuestDetails([FromBody] GuestDetails_sf guestDetailsRequest)
        {
            _salesforceService.InsertGuestDetailsRecord(guestDetailsRequest);
        }

        [HttpPost("UploadImage")]
        public ActionResult UploadImage(IFormFile file)
        {
            // we can put rest of upload logic here.
            return Ok();
        }
    }
}
