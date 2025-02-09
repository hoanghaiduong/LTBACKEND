using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LTBACKEND.Utils.Enums;

namespace LTBACKEND.Entities
{
    public class Payment : BaseIdentity<int>
    {


        [Required] // BookingId là bắt buộc
        public int BookingId { get; set; }

        [Required] // Amount là bắt buộc
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;


        public EStatus Status { get; set; } = EStatus.Pending; // Mặc định là "Pending"


        // Quan hệ với bảng Bookings qua BookingId
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; } // Booking là lớp Entity khác mà bạn đã tạo trước
    }
}
