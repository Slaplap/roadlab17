using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;

namespace Interon.Roadlab.App
{
    public class ProfileViewModel
    {
        public Member Member { get; set; }
        public Client Client { get; set; }

        public ProfileViewModel()
        {
           
         
           
            Member = MemberService.GetMember();
            ClientsService cs = new ClientsService();
            Client = cs.GetCompanyByAccountNumber(MemberService.GetMemberCompanyAccountNumber());
          
        }
    }
    
}