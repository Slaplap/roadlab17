using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Interon.Roadlab.Web.Core.API
{
    public class MainApiController:UmbracoApiController
    {
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool Available()
        {
            return true;
        }
    }


    public class LimsNotificationApiController : UmbracoApiController
    {
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Http.AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Message()
        {
            Debugger.Break();

            try
            {
                if(Request.Headers.GetValues("Authorization").ToString().ToLower() != "test")
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(HttpStatusCode.Unauthorized);
                }
             
            }
            catch
            {

            }
            finally
            {
                
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }
    }
}