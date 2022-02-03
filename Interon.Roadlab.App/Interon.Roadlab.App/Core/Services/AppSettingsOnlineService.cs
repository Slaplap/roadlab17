using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core.Dto;

namespace Interon.Roadlab.App.Core.Services
{
    public static class AppSettingsOnlineService
    {
        public static async Task<AppSettingsDto> GetAppSettings()
        {
            try
            {
                var fUrl = new Url( Interon.Roadlab.Core.Environment.CMSAPI_APPSETTINGS_URL_ENDPOINT );
                
                var appSettings = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken).WithTimeout(Globals.Timeout).GetAsync().ReceiveJson<AppSettingsDto>().ConfigureAwait(true);
                return appSettings;
            }
            catch (FlurlHttpTimeoutException te)
            {
                ErrorService.ErrorExceptionAndAnalytics("Timeout Getting AppSettings", "GetAppSettings()", te);
                return new AppSettingsDto();
            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error Getting AppSettings", "GetAppSettings()", err);
                return  new AppSettingsDto();
            }
            return new AppSettingsDto();


        }
        public static async Task<DateTime> GetAppSettingsUpdateDate()
        {

            DateTime date = new DateTime();
            try
            {
                var fUrl = new Url(Interon.Roadlab.Core.Environment.CMSAPIURL + "/AppSettingsApi/" +
                                   "GetAppSettingsUpdateDate");
               

                date = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken).WithTimeout(Globals.Timeout)
                    .GetAsync().ReceiveJson<DateTime>().ConfigureAwait(true);
                return date;
            }
            catch (Exception e)
            {
                return date;
            }




        }

        public static async Task<bool> SetUpToDate()
        {
            try
            {

                var appSettingsUpdateDate = await GetAppSettingsUpdateDate().ConfigureAwait(true);
                SecureStorageService.AppSettingsDate = appSettingsUpdateDate;
            }
            catch
            {
                return false;
            }

            return true;
        }


        public static async Task<bool> IsUpToDate()
        {
            try
            {
                var appSettingsUpdateDate = await GetAppSettingsUpdateDate();
                if (MyDateHelpers.CompareDayMonthYearHourMinuteSecond(SecureStorageService.AppSettingsDate, appSettingsUpdateDate))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }


        }
    }
}
