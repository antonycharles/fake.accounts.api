using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Entities;
using Accounts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Seeds
{
    public class ProfileRoleSeeder
    {
        public static void Seed(AccountsContext context)
        {
            var listDb = context.ProfilesRoles.ToList();
            
            foreach(var item in list){
                var itemDb = listDb.FirstOrDefault(s => s.ProfileId == item.ProfileId && s.RoleId == item.RoleId);
                if(itemDb == null)
                {
                    context.ProfilesRoles.Add(item);
                }
                else
                {
                    itemDb.ProfileId = item.ProfileId;
                    itemDb.RoleId = item.RoleId;
                    context.Entry(itemDb).State = EntityState.Modified;
                }
            }

            foreach(var item in listDb.Where(s => !list.Select(s => s.ProfileId).Contains(s.ProfileId) && !list.Select(s => s.RoleId).Contains(s.RoleId)))
                context.ProfilesRoles.Remove(item);


            context.SaveChanges();
        }
        
        private static List<ProfileRole> list = new List<ProfileRole>
        {
            new ProfileRole { 
                ProfileId = new Guid("bef292cf-f1b7-4370-b0a3-e00ff099daa3"), 
                RoleId = new Guid("9efb4d43-2286-4588-8129-f87604fb02f2")
            },
            new ProfileRole { 
                ProfileId = new Guid("bef292cf-f1b7-4370-b0a3-e00ff099daa3"), 
                RoleId = new Guid("9f23dc89-c584-442c-ae49-dbbbe95466b7")
            },

            new ProfileRole { 
                ProfileId = new Guid("e63f5f4b-a7b3-455a-8531-652a414e16af"), 
                RoleId = new Guid("4cccfb46-5aee-4c8a-90ac-9dde16e9dee5")
            },
        };  
    }
}