using System.Collections.Generic;
using System.Threading.Tasks;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.DTO.Quicktype;

namespace Interon.Roadlab.LIMS.Services
{
    public static class ServicesService
    {
        public static async Task<Dictionary<string, SageCategory>> ServiceCategories(InputServiceCategories inputServiceCategories)
        {
            var result = await LIMSOnlineService.ServiceCategories(inputServiceCategories);
            return result;
        }

        public static async Task<Dictionary<string, Variants>> VariantsByCategory(InputVariantByCategory input)
        {
           var response = await  LIMSOnlineService.VariantsByCategory(input);
           return response;
        }

        public static async Task<RootSites> SiteByClientOrSearchAsync( string q="", int clientId =0, int page =1,int pageSize = 1000)
        {
            var response = await LIMSOnlineService.SitesByClientOrSearch(clientId,q,page,pageSize);
            return response;
        }
    }
}