using System.Collections.Generic;
using System.Threading.Tasks;
using Interon.Roadlab.LIMS.DTO;

namespace Interon.Roadlab.LIMS.Services
{
    public static class ServiceRequestService
    {
        public static async Task<List<RootClientRequest>> GetRequestByClientOrContactOrSite(InputByClientIdOrContactIdOrSiteId clientIdOrContactIdOrSiteId)
        {
            return await LIMSOnlineService.RequestListByClientOrContactOrSite(clientIdOrContactIdOrSiteId);
        }


        public static async Task<RootClientRequest> GetRequestById(int requestId)
        {
            return await LIMSOnlineService.GetRequestById(requestId);
        }
    }
}