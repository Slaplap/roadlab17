using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Dto;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
 

namespace Interon.Roadlab.App.Core.Services
{
    public static class MemberOnlineService
    {
         

        public static async Task<MemberDto> GetMemberByEmail(string email)
        {

            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_MEMBERSHIPBYEMAIL_URL_ENDPOINT );

            fUrl.SetQueryParams(new
            {
                email = email.E("qz2rg4"),

            });

            try
            {
                var _member = await fUrl.WithTimeout(Globals.Timeout).GetAsync().ReceiveJson<MemberDto>().ConfigureAwait(true);
                SecureStorageService.MemberId = _member.Id;
                return _member;

            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error getting member", "GetMEmberByEmail", err);
                return null;
            }
            
        }
        public static async Task<MemberDto> CreateMember(string email, string name, string surname, string accountNumber, string cellNumber, string password)
        {

            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_CREATEMEMBER_URL_ENDPOINT);

            fUrl.SetQueryParams(new
            {
                email = email.E("qz2rg4"),
                name = name.E("qz2rg4"),
                surname = surname.E("qz2rg4"),
                accountNumber = accountNumber.E("qz2rg4"),
                cellNumber = cellNumber.E("qz2rg4"),
                password = password.E("qz2rg4")


            });

            try
            {
                var _member = await fUrl.WithTimeout(Globals.Timeout).GetAsync().ReceiveJson<MemberDto>().ConfigureAwait(true);

                if(!string.IsNullOrWhiteSpace(_member.Message))
                {
                    ErrorService.EventAndAnalytics("Member message from server :" + _member.Message,AnalyticsService.EventCategory.Info);
                    return null;
                }
                return _member;

            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error getting member", "CreateMember", err);
                return null;
            }

        }
        public static async Task UpdateDeviceId(string email, string deviceId)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_SETMEMBERDEVICEID_URL_ENDPOINT);
            try
            {
                Xamarin.Essentials.VersionTracking.Track();
               
                await fUrl.WithTimeout(10).PostJsonAsync(new  DeviceDto()
                {
                    email = email, 
                    DeviceID = deviceId,
                    OS = Xamarin.Essentials.DeviceInfo.Platform.ToString(),
                    Build = Xamarin.Essentials.VersionTracking.CurrentBuild,
                    Version = Xamarin.Essentials.VersionTracking.CurrentVersion,
                    OsVersion = Xamarin.Essentials.DeviceInfo.Version.ToString(),
                    NotificationHubToken = SecureStorageService.NotificationHubToken
                }).ConfigureAwait(true);



            }
            catch (FlurlHttpTimeoutException fex)
            {
               
                ErrorService.ErrorExceptionAndAnalytics("Connection timing out", "UpdateDeviceId()", fex);



            }
            catch (FlurlHttpException ex)
            {

                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to server", "UpdateDeviceId()", ex);


            }
            catch (Exception e)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to server", "UpdateDeviceId()", e);

            }
        }
        public static async Task RequestOTP(string email)
        {
            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_REQUESTOTP_URL_ENDPOINT);
            try
            {
                 

                fUrl.SetQueryParams(new
                {
                    email = email.E("qz2rg4"),

                });
                 
                await fUrl.WithTimeout(Globals.Timeout).GetAsync().ConfigureAwait(true);


            }
            catch (FlurlHttpTimeoutException fex)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Connection timing out", "UpdateDeviceId()", fex);



            }
            catch (FlurlHttpException ex)
            {

                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to server", "UpdateDeviceId()", ex);


            }
            catch (Exception e)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to server", "UpdateDeviceId()", e);

            }
        }
        public static async Task<bool> SetMemberPassword(string email,string password,string otp)
        {

            var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPI_SETPASSWORD_URL_ENDPOINT);

            fUrl.SetQueryParams(new
            {
                email = email = email.E("qz2rg4"),
                password = password,
                otp = otp

            });

            try
            {
                var success = await fUrl.WithTimeout(Globals.Timeout).GetAsync().ReceiveJson<bool>().ConfigureAwait(true);

                return success;

            }
            catch (Exception err)
            {
                Debugger.Break();
                ErrorService.ErrorExceptionAndAnalytics("Error setting member password", "SetMemberPassword", err);
                return false;
            }

        }
    }
}
