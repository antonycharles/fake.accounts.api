using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Core.Entities
{
    public class ProfileRole
    {
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}