using Interon.Roadlab.LIMS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interon.Roadlab.LIMS.Services
{
    public static class ClientsAndContactsService
    {
       public static async Task<RootClients> ClientsAsync(string q="",int page=1,int pagesize = 10000)
        {
            try
            {


                var companies = await LIMSOnlineService.ClientListBySearch(q, page, pagesize);
                return companies;
            }
            catch (Exception e)
            {
                throw e;
            }
            
          
        }
        public static async Task<Client> ClientByAccountNumberAsync(string accountNumber)
        {
            var companies = await ClientsAsync(accountNumber, 1, 1);
            return companies.CompanyList.FirstOrDefault();
        }

        public static async Task<List<Contact>> ListOfContactsByClientAsync(int companyId)
        {
            var contactsRoot = await LIMSOnlineService.ContactListByCompany(companyId);
            return contactsRoot.ContactsList;
        }

        public static async Task <bool> CreateContact(InputByClientIdAndContact cc )
        {
            var resultCode = await LIMSOnlineService.CreateContact(cc);
            
            if (resultCode.message == "Registered contact successfully.")
            {
                return true;
            }
            return false;
        }

        public static async Task<RootClientRequest> NewRequest(InputNewRequest cc)
        {
            var result = await LIMSOnlineService.NewRequest(cc);

            
            return result;
        }
    }
}
