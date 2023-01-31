using Accounts.Core.Entities;
using Accounts.Core.Enums;
using Accounts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Seeds
{
    public class RoleSeeder
    {
        public static void Seed(AccountsContext context)
        {
            var listDb = context.Roles.ToList();
            
            foreach(var item in list){
                var itemDb = listDb.FirstOrDefault(s => s.Id == item.Id);
                if(itemDb == null)
                {
                    context.Roles.Add(item);
                }
                else
                {
                    itemDb.Name = item.Name;
                    itemDb.Slug = item.Slug;
                    itemDb.IsSystem = item.IsSystem;
                    itemDb.Status = item.Status;
                    itemDb.UpdatedAt = DateTime.UtcNow;
                    context.Entry(itemDb).State = EntityState.Modified;
                }
            }

            foreach(var item in listDb.Where(s => !list.Select(s => s.Id).Contains(s.Id)))
                context.Roles.Remove(item);


            context.SaveChanges();
        }

        private static List<Role> list = new List<Role>
        {
            //Accounts
            new Role { 
                Id = new Guid("9efb4d43-2286-4588-8129-f87604fb02f2"), 
                Name = "Authorization",
                Slug = "authorization",
                AppId = new Guid("bef292cf-f1b7-4370-b0a3-e00ff099daa3"),
                IsSystem = true,
                Status = StatusEnum.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Role { 
                Id = new Guid("9f23dc89-c584-442c-ae49-dbbbe95466b7"), 
                Name = "Authorization Token",
                Slug = "authorization.token",
                AppId = new Guid("bef292cf-f1b7-4370-b0a3-e00ff099daa3"),
                IsSystem = true,
                FatherId = new Guid("9efb4d43-2286-4588-8129-f87604fb02f2"),
                Status = StatusEnum.Active,
                CreatedAt = DateTime.UtcNow
            },

            //Store
            new Role { 
                Id = new Guid("4cccfb46-5aee-4c8a-90ac-9dde16e9dee5"), 
                Name = "User panel",
                Slug = "user.panel",
                AppId = new Guid("d582144d-ee4f-4249-be65-38d8836c1036"),
                IsSystem = false,
                Status = StatusEnum.Active,
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}