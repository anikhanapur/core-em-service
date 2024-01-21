using CoreEventManagementService.Models;

namespace CoreEventManagementService.Services
{
    public interface ISalesforceService
    {
        public Task<string> GetSalesforceAuthToken();
        public void InsertGuestDetailsRecord(GuestDetails_sf guestDetailsRequest);
    }
}
