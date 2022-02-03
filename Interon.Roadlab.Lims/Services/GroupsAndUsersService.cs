using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Interon.Roadlab.LIMS.DTO;

namespace Interon.Roadlab.LIMS.Services
{
 
    public class GroupsAndUsersService
    {
        
            public static async Task<RootUsersInGroup> ListUsersInGroup(int groupId, string q)
            {
                var contactsRoot = await LIMSOnlineService.UserListInGroup(groupId,q);
                return contactsRoot;
            }

            public static async Task<RootGetGroupAndChildren> GetGroupsAndChildren(int groupId)
            {
                var result = await LIMSOnlineService.GetGroupsAndChildren(groupId);
                return result;
            }
            public static async Task<RootStatusModel> UpdateUser(InputUsers inputUsers,int id)
            {
                var result = await LIMSOnlineService.UpdateUser(inputUsers,id);
                return result;
            }
            public static async Task<object> GetUserById(int id)
            {
                var result = await LIMSOnlineService.GetUsrById(id);
                return result;
            }


    }
}
