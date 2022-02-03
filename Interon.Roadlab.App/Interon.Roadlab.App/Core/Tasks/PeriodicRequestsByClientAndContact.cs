using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicRequestsByClientAndContact : IPeriodicTask
    {
         
        public TimeSpan Interval { get; set; }

        public PeriodicRequestsByClientAndContact(int seconds)
        {
           
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public PeriodicRequestsByClientAndContact()
        {
        }
        public async Task<bool> StartJob()
        {

            if (Globals.StoppedBackgroundServices.Contains(this.GetType().Name) || Globals.RunningServices.Contains(this.GetType().Name))
            {
                return true;
            }
            

            if (!SecureStorageService.HasAllLocalLoginCredentials())
            {
                return true;
            }

            var connectionToServer =  OnlineService.HasConnectionToServerAsync();
            if ( !connectionToServer)
            {
                return true;
            }
            
           
            await GetAllRequestsByClientAndContact().ConfigureAwait(true);
            return true;
        }
        

        



        private async Task GetAllRequestsByClientAndContact()
        {

            try
            {
                Globals.RunningServices.Add(this.GetType().Name);
                RequestService requestService = new RequestService();

                var allRequestsByClientAndContact = await RequestsOnlineService.GetRequestsByClientOrContactOrSite(SecureStorageService.ClientId, SecureStorageService.ContactId).ConfigureAwait(true);

              


                if (allRequestsByClientAndContact == null)
                {
                    
                    return;
                }

                foreach (var rootClientRequest in allRequestsByClientAndContact)
                {
                    requestService.CreateOrUpdateClientRequest(rootClientRequest);
                }



                MessagingCenter.Send<object>(new Object(), MessagingCenterValues.RequestChange);
                Globals.StoppedBackgroundServices.Add(this.GetType().Name);


            }
            catch (Exception e)
            {
                
                MessagingCenter.Send<object>(new Object(), MessagingCenterValues.RequestChange);
                ErrorService.ErrorExceptionAndAnalyticsMessageCenter("Error getting Client Requests By Client and Contact", "GetRequestsByClientOrContactOrSite()", e);
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }
        }

       
    }
}