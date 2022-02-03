using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.Services;
using Matcha.BackgroundService;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class CatAndSub
    {
        public long CatId { get; set; }  
        public string CatName { get; set; }
        public long SubId { get; set; }
        public string SubName { get; set; }
        public string VarName { get; set; }
        public long VarId { get; set; }
    }
    public class PeriodicCategories : IPeriodicTask
    {
        public PeriodicCategories(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public TimeSpan Interval { get; set; }

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
            if (!OnlineService.HasInternet())
            {
                return true;
            }


            try
            {
                Globals.RunningServices.Add(this.GetType().Name);

                await GetCategories();
                MessagingCenter.Send<object>(new Object(), MessagingCenterValues.CategoriesChange);
                return StopThisTask();
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }

        }

        private async Task GetCategories()
        {
            
            SecureStorageService.Categories = await LIMSOnlineService.ServiceCategories(new InputServiceCategories()
            {
                RoleId = 136163,
                ParentId = 144048,
                ProviderGroupId = 506
            } ).ConfigureAwait(true);
            
             
        }

        private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
    }
}