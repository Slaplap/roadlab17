using System.Collections.Generic;

namespace Interon.Roadlab.Core.Dto
{
    public class MemberDto

    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CellNo { get; set; }
        public string Email { get; set; }
        public  List<ClientDto> Clients { get; set; }=new List<ClientDto>();
        public string Message { get; set; }



    }
}
