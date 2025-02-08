using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LTBACKEND.Entities
{
    public class User : BaseIdentity<int>
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress] // Đảm bảo Email hợp lệ nếu có
        [StringLength(255)] // Giới hạn độ dài của Email
        public string Email { get; set; }


        [StringLength(255)] // Giới hạn độ dài của PasswordHash
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [StringLength(20)] // Giới hạn độ dài của PhoneNumber
        public string PhoneNumber { get; set; }


        [StringLength(50)] // Giới hạn độ dài của FullName
        public string FirstName { get; set; }


        [StringLength(50)] // Giới hạn độ dài của FullName
        public string LastName { get; set; }
        public bool? EmailVerified { get; set; } = false;
        public string? Avatar { get; set; } = null!;
        [JsonIgnore]
        public string? RefreshToken { get; set; } = null!;
        public bool IsDisabled { get; set; } = false;
        public DateTime? LastLogin { get; set; } = DateTime.Now;
        // Quan hệ 1-N với bảng Bookings (Một User có thể có nhiều Booking)
        [JsonIgnore]
        public virtual Hotel? Hotel { get; set; } = null!;
        [JsonIgnore]
        public virtual IList<UserRoles> UserRoles { get; set; } = [];
        [JsonIgnore]
        public virtual IList<Booking> Bookings { get; set; } = [];

    }
}
