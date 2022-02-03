using Flurl;
using Flurl.Http;
using Interon.Roadlab.Core.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interon.Roadlab.App.Core.Services
{
    public static class BranchesOnlineService
    {
        public static async Task<List<BranchDto>> GetBranches()
        {
            try
            {
                var fUrl = new Url(OnlineService.BASEURL + OnlineService.PATH + "/BranchesApi/" + "GetAllBranches");
                List<BranchDto> branches = new List<BranchDto>();
                var receiveJsonList = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken)
                    .WithTimeout(Globals.Timeout).GetJsonAsync<BranchesDto>().ConfigureAwait(true);

                return receiveJsonList.branchDtos;
            }
            catch (FlurlHttpTimeoutException te)
            {
                ErrorService.ErrorExceptionAndAnalytics("Timeout Getting Branches", "GetBranches()", te);
                return new List<BranchDto>();
            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error Getting Branches", "GetBranches()", err);
                return new List<BranchDto>();
            }
            return new List<BranchDto>();
        }

        public static async Task<DateTime> GetBranchesUpdateDate()
        {
            DateTime date = new DateTime();
            try
            {
                var fUrl = new Url(OnlineService.BASEURL + OnlineService.PATH + "/BranchesApi/" +
                                   "GetBranchesUpdateDate");
                int attempt = 0;

                date = await fUrl.WithOAuthBearerToken(SecureStorageService.RefreshToken).WithTimeout(Globals.Timeout)
                    .GetAsync().ReceiveJson<DateTime>().ConfigureAwait(true);
                return date;
            }
            catch (FlurlHttpTimeoutException te)
            {
                ErrorService.ErrorExceptionAndAnalytics("Timeout Getting Branches Update Date", "GetBranchesUpdateDate()", te);
            }
            catch (Exception err)
            {
                ErrorService.ErrorExceptionAndAnalytics("Error Getting Branches Update Date", "GetBranchesUpdateDate()", err);
                return date;
            }

            return date;
        }

        public static async Task<bool> SetUpToDate()
        {
            try
            {
                var branchesUpdateDate = await GetBranchesUpdateDate().ConfigureAwait(true);
                SecureStorageService.BranchesUpdateDate = branchesUpdateDate;
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
                var branchesUpdateDate = await GetBranchesUpdateDate();
                if (MyDateHelpers.CompareDayMonthYearHourMinuteSecond(SecureStorageService.BranchesUpdateDate, branchesUpdateDate))
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