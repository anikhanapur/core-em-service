using CoreEventManagementService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreEventManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ISalesforceService _salesforceService;
        public AuthController(ISalesforceService salesforceService)
        {
            _salesforceService = salesforceService;
        }

        [HttpPost(Name = "GetAuthToken")]
        public Task<string> GetAuthToken()
        {
            return _salesforceService.GetSalesforceAuthToken();
        }
    }
}
