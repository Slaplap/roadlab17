using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core.Dto;


namespace Interon.Roadlab.App.Core.Services
{
    public static class MessageOnlineService
    {

 


        public static async Task<bool> SaveMessageJsonDto(MessageJsonDto messageJsonDto)
        {
            try
            {
                var fUrl = new Url(OnlineService.BASEURL + OnlineService.PATH + "MessagesFrontendApi/" + $"SaveMessageJsonDto");
                List<MessageJsonDto> Orders = new List<MessageJsonDto>();
                    var receiveOrderDto = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken)
                        .WithTimeout(Globals.Timeout).PostJsonAsync(messageJsonDto).ReceiveJson<bool>().ConfigureAwait(true);


                    return receiveOrderDto;
            }
            catch (FlurlHttpTimeoutException te)
            {
                
                ErrorService.ErrorExceptionAndAnalytics("Timeout Setting Notification", "SaveMessageJsonDto()", te);
                throw te;
            }
            catch (Exception err)
            {
                
                ErrorService.ErrorExceptionAndAnalytics("Error Setting Message", "SaveMessageJsonDto()", err);
                throw err;
            }

            return false;
        }
        

    }
}
