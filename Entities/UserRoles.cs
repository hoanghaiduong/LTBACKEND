using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LTBACKEND.Entities
{
   public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
        [JsonIgnore]
        public virtual Role Role { get; set; } = null!;
    }
}