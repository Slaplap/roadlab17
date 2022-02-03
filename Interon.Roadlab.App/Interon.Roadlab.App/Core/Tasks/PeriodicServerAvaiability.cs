
using Matcha.BackgroundService;
using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Interon.Roadlab.App.Core.Services;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicServerAvailability : IPeriodicTask
    {
        
        public PeriodicServerAvailability(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {

            try
            {
                await CheckServer();
                await CheckLims();
                await CheckBoth();
                return true;
            }
            finally
            {
                
            }
        }

        private async Task CheckBoth()
        {
            if (Globals.IsLIMSAvaiable && Globals.IsCMSAvailable)
            {
                Globals.IsServerAvaiable = true;
            }
            else
            {
                Globals.IsServerAvaiable = false;
            }
        }

        private async Task CheckLims()
        {
            try
            {
                //only change status when there is a status change
                if (Globals.IsLIMSAvaiable == false && await Interon.Roadlab.LIMS.Services.LIMSOnlineService.IsServerOnlineAsync())
                {
                    Globals.IsLIMSAvaiable = true;
                }
                if (Globals.IsLIMSAvaiable == true && !await Interon.Roadlab.LIMS.Services.LIMSOnlineService.IsServerOnlineAsync())
                {
                    Globals.IsLIMSAvaiable = false;
                }
            }
            catch (Exception e)
            {
                Globals.IsLIMSAvaiable = false;
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to LIMS", "PeriodicServerAvailability()", e);
            }
            
        }

        public static async Task CheckServer()
        {
            if (!OnlineService.HasInternet())
            {
                Globals.IsCMSAvailable = false;
            }
             
            if (Globals.IsCMSAvailable == false)
            {
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicClients.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicAppSettings.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicBackgroundServiceAnalytics.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicRequestsByClient.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicRequestsByClientAndContact.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicSites.ToString());
                Globals.StoppedBackgroundServices.Add(BackgroundServices.PeriodicCategories.ToString());

            }
             


            try
            {
              
                //only change status when there is a status change
                if (Globals.IsCMSAvailable == false && await Interon.Roadlab.CMS.CmsOnlineService.CheckCMSAsync())
                {
                    Globals.IsCMSAvailable = true;
                }
                //only change status when there is a status change
                if (Globals.IsCMSAvailable == true && !await Interon.Roadlab.CMS.CmsOnlineService.CheckCMSAsync())
                {
                    Globals.IsCMSAvailable = false;
                }

            }
            catch (Exception e)
            {
                Globals.IsCMSAvailable = false;
                ErrorService.ErrorExceptionAndAnalytics("Connection Error to CMS", "PeriodicServerAvailability()", e);
            }
             

          


        }

        

      
    }

}
