using System.Collections.Generic;
using System.Diagnostics;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Interon.Roadlab.LIMS.Test
{
    [TestClass]
    public class LIMSUnitTest1
    {

        [TestMethod]
        public async Task IsServerOnlineAsync()
        {
            Assert.IsTrue(await LIMSOnlineService.IsServerOnlineAsync());

        }

        [TestMethod]
        public async Task Clients()
        {
            var companies = await ClientsAndContactsService.ClientsAsync("Roadlab");
            Assert.IsNotNull(companies);
            Assert.IsTrue(companies.CompanyList.Any());

        }
        [TestMethod]
        public async Task ClientByAccountNumber()
        {
            var company = await ClientsAndContactsService.ClientByAccountNumberAsync("GR144O001");

            Assert.IsNotNull(company);
            Assert.IsTrue(company.Name == "144 Oxford Road JV");

        }
        [TestMethod]
        public async Task ClientContacts()
        {
            var contactList = await ClientsAndContactsService.ListOfContactsByClientAsync(1992);

            Assert.IsNotNull(contactList);
            Assert.IsTrue(contactList.Any());


        }

        [TestMethod]
        public async Task CreateContact()
        {
            var cc = new InputByClientIdAndContact()
            {
                ClientId = 1992,
                Contact = new Contact
                {
                    Surname = "test",
                    Firstname = "test",
                    Email = "test2@interon.co.za",
                    MobilePhone = "0833267925"
                }
            };
            var result = await ClientsAndContactsService.CreateContact(cc);


            Assert.IsTrue(result);


        }
        [TestMethod]
        public async Task NewRequest()
        {
            var cc = new InputNewRequest()
            {
                Contact = 70670,
                Branch = 506,
                CompanyName = "ACME Industries X",
                ContactName = "James",
                ContactSurname = "Doe",
                ContactEmail = "johndoe@acme.com",
                Next = "distribute",
                RequestDetails = "test",
                SiteAddress = 10783,
                Services = new List<InputNewRequestService>()



            };
            var result = await ClientsAndContactsService.NewRequest(cc);

            Assert.IsNotNull(result);


        }
        [TestMethod]
        public async Task UpdateContact()
        {
            var cc = new InputByClientIdAndContact()
            {
                ClientId = 1992,
                Contact = new Contact
                {
                    Id = 70690,
                    Surname = "test",
                    Firstname = "test",
                    Email = "test3@interon.co.za",
                    MobilePhone = "0833267925"
                }
            };
            var result = await ClientsAndContactsService.CreateContact(cc);


            Assert.IsTrue(result);


        }
        [TestMethod]
        public async Task RequestListByClientAndContact()
        {
            var request = new InputByClientIdOrContactIdOrSiteId()
            {
                ClientId = 1992,
                ContactId = 62161

            };
            var result = await ServiceRequestService.GetRequestByClientOrContactOrSite(request);

                Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public async Task RequestListByClientAndSite()
        {
            var request = new InputByClientIdOrContactIdOrSiteId()
            {
                ClientId = 1992,
                SiteId = 10995


            };
            var result = await ServiceRequestService.GetRequestByClientOrContactOrSite(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public async Task RequestListByClient()
        {
            var request = new InputByClientIdOrContactIdOrSiteId()
            {
                ClientId = 1992
                 


            };
            var result = await ServiceRequestService.GetRequestByClientOrContactOrSite(request);
            Debugger.Break();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public async Task RequestByById()
        {
            int requestId = 697;
            var result = await ServiceRequestService.GetRequestById(requestId);
            Debugger.Break();
            Assert.IsNotNull(result);
          
        }
        [TestMethod]
        public async Task RequestListByClientAndContactAndSite()
        {
            var request = new InputByClientIdOrContactIdOrSiteId()
            {
                ClientId = 1992,
                SiteId = 10995,
                ContactId = 62161




            };
            var result = await ServiceRequestService.GetRequestByClientOrContactOrSite(request);
            Debugger.Break();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public async Task ListUsersInGroup()
        {

            var result = await GroupsAndUsersService.ListUsersInGroup(506, "");
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public async Task GetGroupsAndChildren()
        {

            var result = await GroupsAndUsersService.GetGroupsAndChildren(506);
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public async Task ServiceCategories()
        {

            var result = await ServicesService.ServiceCategories(new InputServiceCategories()
            {
                RoleId = 136163,
                ParentId = 144048, //(imported from sage) 144048 -> (Testing) 144217 -> (Aggregate - Mechanical interlocking and Particle s) 144219,
                ProviderGroupId = 506
            });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());


        }
        [TestMethod]
        public async Task VariantsByCategory()
        {
            var response = await ServicesService.VariantsByCategory(new InputVariantByCategory());
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Any());



        }

        [TestMethod]
        public async Task SiteListByClientOrSearch()
        {
            var response = await ServicesService.SiteByClientOrSearchAsync("",clientId:1992);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sites.Any());



        }
    }
}