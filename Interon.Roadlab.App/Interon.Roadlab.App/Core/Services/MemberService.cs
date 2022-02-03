using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.Core.Dto;
using Newtonsoft.Json;
using SQLite;

namespace Interon.Roadlab.App.Core.Services
{
     

    public static class  MemberService  
    {
       

        public static Member DtoToMember(Interon.Roadlab.Core.Dto.MemberDto memberDtoDto)
        {
            return new Member(){
                CellNo = memberDtoDto.CellNo,
                Clients = JsonConvert.SerializeObject(memberDtoDto.Clients),
                Email = memberDtoDto.Email,
                Id=memberDtoDto.Id,
                Name = memberDtoDto.Name,
                Surname = memberDtoDto.Surname};
        }

        public static MemberDto MemberToDto(Member member)
        {
            return new MemberDto(){CellNo = member.CellNo,
                Clients = JsonConvert.DeserializeObject<List<ClientDto>>(member.Clients) ,
                Email = member.Email,Id = member.Id,
                Name = member.Name,
                Surname = member.Surname};
        }
        public static bool IsRegisteredOnLocalDevice()
        {
            try
            {
                // var registered = _db.Query<Member>("SELECT * FROM Member").Any();
                if (SecureStorageService.Member != null)
                {
                    try
                    {


                        var result = AuthenticationService.HasValidCredentialsAsync().GetAwaiter().GetResult();
                        //if (!result)
                        //{
                        //    Globals.NewApp();
                        //}

                        //  Debugger.Break();
                        return result;
                    }
                    catch
                    {
                        Debugger.Break();
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static Member GetMember()
        {
            try
            {
                //SecureStorageService.Member = new Member();
                //var member =  _db.Query<Member>("SELECT * FROM Member").FirstOrDefault();
                //if (member == null)
                //{
                //  throw  new Exception("Member is null");
                //}
                //return member;
                return SecureStorageService.Member;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new Member();

        }
        public static void CreateOrUpdateMember(Member member)
        {
            try
            {
                //if (IsRegisteredOnLocalDevice())
                //{
                //    _db.Update(member);
                //}
                //else
                //{
                //    _db.Insert(member);
                //}
                SecureStorageService.Member = member;
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
           
        }
        public static void DeleteMember()
        {
           // _db.Execute("Delete from Member where 1=1");
           SecureStorageService.Member = null;
        }


        public static string GetMemberCompanyAccountNumber()
        {
            return SecureStorageService.ClientAccountNumber;
        }

        
    }
}
