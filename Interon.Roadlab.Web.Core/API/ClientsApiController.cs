using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Interon.Roadlab.Web.Core.API
{

    
    public class ClientsApiController : UmbracoApiController
    {
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool TestGet()
        {
            return true;
        }
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<LIMS.DTO.Client> GetClientByAccountNumber(string accountNumber)
        {

            var company = await  LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(accountNumber);
            return company;

        }
      
    }
}