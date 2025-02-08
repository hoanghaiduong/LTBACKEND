using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTBACKEND.Entities
{
    public class Review : BaseIdentity<int>
    {
        [Required] // BookingId là bắt buộc
        public int BookingId { get; set; }

        [Required] // Rating là bắt buộc
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500)] // Giới hạn độ dài của Comment là 500 ký tự
        public string Comment { get; set; }

        // Quan hệ với bảng Bookings qua BookingId
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; } // Booking là lớp Entity khác đã được định nghĩa
    }
}
