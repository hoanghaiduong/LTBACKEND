using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LTBACKEND.Utils.Enums;

namespace LTBACKEND.Entities
{
    public class Booking : BaseIdentity<int>
    {
        [Required] // UserId là bắt buộc
        public int UserId { get; set; }

        [Required] // RoomId là bắt buộc
        public int RoomId { get; set; }

        [Required] // CheckInDate là bắt buộc
        public DateTime CheckInDate { get; set; }

        [Required] // CheckOutDate là bắt buộc
        public DateTime CheckOutDate { get; set; }

        [Range(minimum: 1, maximum: 3)]
        public EStatus Status { get; set; } = EStatus.Pending; // Mặc định là "Pending"

        // Quan hệ với bảng Users
        [ForeignKey("UserId")]
        public User User { get; set; } // User là class Entity khác đã được định nghĩa

        // Quan hệ với bảng Rooms
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // Room là class Entity khác đã được định nghĩa
        [JsonIgnore]
        public virtual IList<Payment> Payments { get; } = [];

        [JsonIgnore]
        public virtual IList<Review> Reviews { get; } = [];
    }
}
