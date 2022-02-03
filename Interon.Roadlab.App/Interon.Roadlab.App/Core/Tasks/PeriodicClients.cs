using Interon.Roadlab.App.Core.Services;
using Matcha.BackgroundService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Interon.Roadlab.App.Core.Tasks
{
    public class PeriodicClients : IPeriodicTask
    {
        public PeriodicClients(int seconds)
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
                ClientsService clientsService = new ClientsService();
           
                if (!string.IsNullOrWhiteSpace(SecureStorageService.ClientAccountNumber) && clientsService.GetCompanies().Count >= 1)
                {
                    return StopThisTask();
                }

            

                var connectionToServerAsync =  OnlineService.HasConnectionToServerAsync() ;
                if (!connectionToServerAsync)
                {
                    return true;
                }

            
                var accountNumber = JsonConvert.DeserializeObject<Dictionary<string, string>>(MemberService.GetMember().Clients).FirstOrDefault().Key;

                var companyDto = await  LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(accountNumber);
                if (companyDto != null)
                {
                    ClientsService cs = new ClientsService();
                    cs.CreateOrUpdateCompany(cs.DtoToCompany(companyDto));
                    SecureStorageService.ClientAccountNumber = companyDto.AccountNumber.ToString();
                }

                return StopThisTask();
            }
            finally
            {
                Globals.RunningServices.RemoveAll(x => x == this.GetType().Name);
            }
           
        }
        private bool StopThisTask()
        {
            Globals.StoppedBackgroundServices.Add(this.GetType().Name);
            return false;
        }
    }
}