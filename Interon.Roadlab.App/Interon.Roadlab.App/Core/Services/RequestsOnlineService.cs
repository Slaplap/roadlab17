using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.Core.Models;
using Interon.Roadlab.LIMS.DTO;

namespace Interon.Roadlab.App.Core.Services
{
    public static class RequestsOnlineService
    {
        public static async Task<List<RootClientRequest>> GetRequestsByClientOrContactOrSite(int clientId = 0, int contactId =0 ,int siteId =0)
        {
            var rootClientRequests =  await LIMS.Services.ServiceRequestService.GetRequestByClientOrContactOrSite(new InputByClientIdOrContactIdOrSiteId()
            {
                ClientId = clientId,
                ContactId = contactId,
                SiteId = siteId 
            });
            return rootClientRequests;
        }
        

    }
}