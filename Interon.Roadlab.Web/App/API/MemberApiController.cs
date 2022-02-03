using Interon.Roadlab.Core;
using Interon.Roadlab.Core.Dto;
using Our.Umbraco.AuthU;
using Our.Umbraco.AuthU.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.Services;
using Interon.Roadlab.Web.App.Interfaces;
using Interon.Roadlab.Web.App.Services;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;
using Company = Umbraco.Web.PublishedModels.Company;

namespace Interon.Roadlab.Web.App.API
{
    [OAuth]
    public class MemberApiController : UmbracoApiController
    {
       
        private IMembershipService _membershipService;

        public MemberApiController( IMembershipService membershipMemberService)
        {
           
            _membershipService = membershipMemberService;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool TestGet()
        {
            var memberService = Services.MemberService;
            var member = memberService.GetByEmail("anton@interon.co.za");
            var clients = member.GetValue("clients");
            return true;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.HttpPost]
        [MemberAuthorize]
        [System.Web.Http.HttpGet][System.Web.Http.HttpPost]
        public bool IsAuthorized()
        {
            return true;
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public Interon.Roadlab.Core.Dto.MemberDto GetMemberByEmail(string email)
        {
             email = email.D("qz2rg4").URLDecode();
            if (email == null)
            {
                return null;
            }
            return CMSMemberByEmail(email);
        }
        public class NestedContent
        {
            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("ncContentTypeAlias")]
            public string NcContentTypeAlias { get; set; }

            [JsonProperty("accountNumber")]
            public string AccountNumber { get; set; }

            [JsonProperty("companyName")]
            public string CompanyName { get; set; }
        }
        private MemberDto CMSMemberByEmail(string email)
        {
            string _email;
            var memberService = Services.MemberService;
            long totalRecords;
            IPublishedContent member;

            try
            {
              
                member = Umbraco.MembershipHelper.GetByEmail(email.Trim());
            }
            catch (Exception ex)
            {
                Logger.Info(typeof(MemberApiController), $"Error finding email {email}");
                return null;
            }

            if (member != null)
            {
                Umbraco.Web.PublishedModels.Member typedMember = new Umbraco.Web.PublishedModels.Member(member);
                var typedClients = typedMember.Clients;
                var clients =  new List<ClientDto>();

                foreach (var typedClient in typedClients)
                {
                    clients.Add(new ClientDto()
                    {
                        AccountNumber = typedClient.LimsClientAccountNumber,
                        Name = typedClient.LimsClientName,
                        ClientId = typedClient.LimsClientId,
                        ContactId = typedClient.LimsContactId


                    });
                }
                

                var returnMember = new Interon.Roadlab.Core.Dto.MemberDto()
                {
                    Name = typedMember.FirstName,
                    CellNo = typedMember.MobileNumber,
                    Clients = clients,
                    Email = email,
                    Surname = typedMember.Surname,
                    Id = typedMember.Id
                };
                return returnMember;
            }
            else
            {
                return null;
            }
        }

      

        [System.Web.Mvc.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<MemberDto> CreateMember(string email, string name, string surname, string cellNumber, string accountNumber, string password)
        {
            string _email = email.D("qz2rg4").URLDecode();
            string _name = name.D("qz2rg4").URLDecode();
            string _surname = surname.D("qz2rg4").URLDecode();
            string _cellNumber = cellNumber.D("qz2rg4").URLDecode();
            string _accountNumber = accountNumber.D("qz2rg4").URLDecode().ToUpper().Trim();
            string _password = password.D("qz2rg4").URLDecode();
            if (_email == null || _name == null || _surname == null || _cellNumber == null || _accountNumber == null || _password == null)
            {
                return new MemberDto()
                {
                    Message = "Please enter the relevant fields"
                };
            }


                var company = await LIMS.Services.ClientsAndContactsService.ClientByAccountNumberAsync(_accountNumber);
            if (company == null)
            {
                return new MemberDto()
                {
                    Message = "Your Account number could not be found"
                };
            }

           
            var member = _membershipService.GetMemberByEmail(_email);


            IMember _member = null;
            
            Contact limsMember;
            if (member == null)
            {
                _member = await _membershipService.CreateMember(_email, _name, _surname, _cellNumber, _accountNumber, _password, company);
                var limsCompanyContacts = await Interon.Roadlab.LIMS.Services.ClientsAndContactsService.ListOfContactsByClientAsync(company.Id);
                limsMember = limsCompanyContacts.Where(x => x.Email == _email).FirstOrDefault();
                if (limsMember == null)
                {
                    await Interon.Roadlab.LIMS.Services.ClientsAndContactsService.CreateContact(new InputByClientIdAndContact()
                    {
                        ClientId = company.Id,
                        Contact = new Contact()
                        {
                            Email = _email,
                            Firstname = _name,
                            Surname = _surname,
                            MobilePhone = _cellNumber,
                            ExternalId = _member.Key.ToString()

                           
                        }
                    });
                }
                else
                {
                    var inputUser = new InputUsers();
                    inputUser.CCB = new CCB();
                    inputUser.UGB = new UGB();
                    inputUser.Users = new Users();
                    inputUser.CCB.ClientId = company.Id;
                    inputUser.UGB.GroupId = 506;
                    inputUser.Users.Email = limsMember.Email;
                    inputUser.Users.Firstname = limsMember.Firstname;
                    inputUser.Users.Surname = limsMember.Surname;
                    inputUser.Users.MobilePhone = _cellNumber;
                    inputUser.Users.ExternalId = _member.Key.ToString();
                    try
                    {
                        RootStatusModel rsm = await LIMS.Services.GroupsAndUsersService.UpdateUser(inputUser, limsMember.Id);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(this.GetType(),"Error Updating LIMS user :" + e.Message);
                    }
                }
                 
              
            }
            else
            {
                return new MemberDto()
                {
                    Message = "This member already exists"
                };
            }

            if (_member == null)
            {
                return new MemberDto()
                {
                    Message = "Error Creating Member"
                };
            }

            if (_member.Id == 0)
            {
                return new MemberDto()
                {
                    Message = "Member could not be created"
                };
            }



            var publishedMember = Umbraco.MembershipHelper.GetById(_member.Id);

            if (publishedMember != null)
            {
                Umbraco.Web.PublishedModels.Member typedMember = new Umbraco.Web.PublishedModels.Member(publishedMember);
                var typedClients = typedMember.Clients;
                List<ClientDto> clients = new List<ClientDto>();
                foreach (var clientDto in typedClients)
                {
                    clients.Add(new ClientDto()
                    {
                        AccountNumber = clientDto.LimsClientAccountNumber,
                        Name = clientDto.LimsClientName,
                        ClientId = clientDto.LimsClientId,
                        ContactId = clientDto.LimsContactId
                       
                    });
                }
                
                var returnMember = new Interon.Roadlab.Core.Dto.MemberDto()
                {
                    Name = typedMember.FirstName,
                    CellNo = typedMember.MobileNumber,
                    Clients =  clients,
                    Email = _email,
                    Surname = typedMember.Surname,
                    Id = typedMember.Id
                };
                return returnMember;
            }
            else
            {
                return null;
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.AcceptVerbs("POST")]
        public HttpStatusCodeResult SetMemberDeviceId([FromBody] DeviceDto deviceDto)
        {
            if (string.IsNullOrWhiteSpace(deviceDto.email) || string.IsNullOrWhiteSpace(deviceDto.DeviceID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var memberService = Services.MemberService;

            IMember member;

            try
            {
                member = memberService.GetByEmail(deviceDto.email);
            }
            catch (Exception ex)
            {
                Logger.Info(typeof(MemberApiController), $"Error finding email {deviceDto.email}");
                return new HttpStatusCodeResult(HttpStatusCode.NonAuthoritativeInformation);
            }

            if (member != null)
            {
                member.SetValue("deviceId", deviceDto.DeviceID);
                member.SetValue("communicationDateTime",DateTime.Now);
                member.SetValue("deviceOS",deviceDto.OS);
                member.SetValue("deviceSoftwareVersion",deviceDto.Version);
                member.SetValue("deviceSoftwareBuild",deviceDto.Build);
                member.SetValue("deviceOSVersion",deviceDto.OsVersion);
                member.SetValue("notificationHubToken",deviceDto.NotificationHubToken);
                memberService.Save(member);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
       
        [System.Web.Http.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool RequestOTP(string email)
        {


            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            string _email=email;
            if (!email.IsValidEmail())
            {
                _email = email.D("qz2rg4").URLDecode();
            }

            if (!_email.IsValidEmail())
            {
                return false;
            }
            if (!_membershipService.RequestOTP(_email))
            {
                Logger.Info(typeof(MemberApiController), $"Error finding email {_email}");
            }

            return true;
        }
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public bool SetPassword(string email, string password, string otp)
        {

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(otp))
            {
                return false;
            }
            string _email = email.D("qz2rg4").URLDecode();
            var memberService = Services.MemberService;


            IMember member;

            try
            {
                member = memberService.GetByEmail(_email);
            }
            catch (Exception ex)
            {
                Logger.Info(typeof(MemberApiController), $"Error finding email {_email}");
                return false;
            }

            if (member != null)
            {
                var _otp = member.GetValue<string>("otp");
                if (otp != _otp || string.IsNullOrWhiteSpace(_otp))
                {
                    return false;
                }
                member.SetValue("otp", "");
                memberService.SavePassword(member, password);
                memberService.Save(member);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}