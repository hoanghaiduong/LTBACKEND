using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LTBACKEND.Entities
{
    public class Role :BaseIdentity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; } = null!;
        [JsonIgnore]
        public virtual IList<UserRoles> UserRoles { get;set; } = [];
    }
}