using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace Interon.Roadlab.CMS
{
    public static class CmsOnlineService
    {
        public static async Task<bool> CheckCMSAsync()
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_AVAILABLE_URL_ENDPOINT);
            try
            {
                var available = await fUrl.WithTimeout(60).GetJsonAsync<bool>().ConfigureAwait(true);

                return available;

            }
            catch (FlurlHttpTimeoutException fex)
            {


                return false;


            }
            catch (FlurlHttpException ex)
            {
                return false;
                 


            }
            catch (Exception e)
            {
                return false;


            }
        }

    }
}