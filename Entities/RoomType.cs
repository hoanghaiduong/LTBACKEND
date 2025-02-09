using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LTBACKEND.Entities
{
    public class RoomType : BaseIdentity<int>
    {
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; } = null!;
        [Range(0.01, double.MaxValue)]
        public decimal PricePerNight { get; set; }
        [Range(0, 10)]
        public int Capacity { get; set; }
      
        public virtual IList<Room> Rooms { get; set; } = [];
    }
}