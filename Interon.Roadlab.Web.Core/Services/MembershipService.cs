using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClientDependency.Core.Logging;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.Web.Core.ContentModels;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Member = Interon.Roadlab.Web.Core.ContentModels.Member;

namespace Interon.Roadlab.Web.Core.Services
{
    public interface IMembershipService
    {
        Task<IMember> CreateMember(string email, string name, string surname, string cellNumber, string accountNumber, string password, Client client);
        IMember GetMemberByEmail(string email);
        bool RequestOTP(string email);
        IMember GetByEmail(string modelLogin);
        IMember GetByMemberId(int memberId);
        void ChangePassword(int memberId, string password);
        IEnumerable<LimsElement> GetCurrentMemberClients();
        Member GetCurrentMember();
    }

    public class MembershipService : IMembershipService
    {
        private readonly ServiceContext _services;
        private ILogger _logger;
        private IMemberService _memberService;
        private UmbracoHelper _umbracoHeper;

        public MembershipService(IMemberService memberService, UmbracoHelper umbracoHelper)
        {
            _memberService = memberService;
            _umbracoHeper = umbracoHelper;
        }
        private List<Dictionary<string, object>> CompaniesField(string email, string accountNumber, string CompanyName, int clientId, int contactId)
        {
            var memberS = _memberService.GetByEmail(email);

            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            if (memberS.HasProperty("clients"))
            {
                try
                {
            items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(memberS.GetValue<string>("clients"));

                }
                catch
                {

                }

            }
            var c = JsonConvert.SerializeObject(items);
            if (items.Any(x => x.ContainsValue(accountNumber)))
            {
                return items;
            }
            var comp = new Dictionary<string, object>()
            {
                {
                    "key", Guid.NewGuid()
                },
                {
                    "ncContentTypeAlias", "limsElement"
                },
                {
                    "name", ""
                },

                {
                    "limsClientAccountNumber",  accountNumber
                },
                {
                    "limsClientName", CompanyName
                },
                {
                    "limsClientId", clientId
                },
                {
                "limsContactId", contactId
                 }


            };

            items.Add(comp);
            return items;
            // memberS.SetValue("clients", JsonConvert.SerializeObject(items));
            // memberService.Save(memberS);
        }

        public async Task<IMember> CreateMember(string email, string name, string surname, string cellNumber, string accountNumber, string password, Client client)
        {
            try
            {


                var _member = _memberService.CreateMember(email, email, name + " " + surname, "member");
                var _contacts = await LIMS.Services.ClientsAndContactsService.ListOfContactsByClientAsync(client.Id);
                Contact _contact = _contacts.FirstOrDefault(x => x.Email == email);
                if (_contact == null)
                {
                    return null;
                }
                _member.Name = name + " " + surname;
                _member.SetValue("firstName", name);
                _member.SetValue("surname", surname);
                _member.SetValue("mobileNumber", cellNumber);
                 
                _memberService.Save(_member);
                var jString = JsonConvert.SerializeObject(CompaniesField(email, client.AccountNumber, client.Name, client.Id, _contact.Id));
                _member.SetValue("clients",jString );

                _memberService.Save(_member);
              
                try
                {
                    _memberService.SavePassword(_member, password);
                }
                catch (Exception ErrorWithSavePassword)
                {
                    return null;
                }
                _memberService.AssignRole(_member.Id, "App Members");
                return _member;
            }
            catch (Exception e)
            {
                
                Debugger.Break();
                return null;
            }

            return null;
        }

        public IMember GetMemberByEmail(string email)
        {
            return _memberService.GetByEmail(email);
        }

        public bool RequestOTP(string email)
        {


            IMember member;

            try
            {

                member = _memberService.GetByEmail(email);
                if (member == null)
                {
                    return false;
                }
                Random rnd = new Random();
                var otp = rnd.Next(1000, 9999);
                member.SetValue("otp", otp);
                _memberService.Save(member);
                var storedCell = member.GetValue("mobileNumber").ToString();
                if (storedCell.Substring(storedCell.Length - 4, 4) == storedCell.Substring(storedCell.Length - 4, 4))
                {
                    SmsService.SendSMS(storedCell, $"Roadlab - Your One Time Pin is : {otp.ToString()}");

                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return false;

            }

            return false;
        }

        public IMember GetByEmail(string modelLogin)
        {
            var membership = _memberService.GetByEmail(modelLogin);
            List<IContent> items = new List<IContent>();


            var content = membership.Properties["clients"].Values;


            return membership;
        }

        public IMember GetByMemberId(int memberId)
        {
            return _memberService.GetById(memberId);
        }

        public void ChangePassword(int memberId, string password)
        {
            _memberService.SavePassword(_memberService.GetById(memberId), password);
            return;
        }

        public IEnumerable<LimsElement> GetCurrentMemberClients()
        {
            var member = _umbracoHeper.MembershipHelper.GetCurrentMember();
            var _member = new  Member(member);
            var memberClients = _member.Clients;
            if (memberClients == null)
            {
                throw new Exception("Member must be linked to a client");
            }

            return memberClients;
        }
        public Member GetCurrentMember()
        {
            return (Member)_umbracoHeper.MembershipHelper.GetCurrentMember();

        }
    }


}