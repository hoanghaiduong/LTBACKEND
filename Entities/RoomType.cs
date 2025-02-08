using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LTBACKEND.Entities
{
    public class RoomType : BaseIdentity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        [JsonIgnore]
        public virtual IList<Room> Rooms { get; set; } = [];
    }
}