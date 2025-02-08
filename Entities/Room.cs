using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LTBACKEND.Utils.Enums;

namespace LTBACKEND.Entities
{
    public class Room : BaseIdentity<int>
    {
        public int HotelID { get; set; }
        public int RoomTypeId { get; set; }

        [Required] // Đảm bảo RoomNumber không được để trống
        [StringLength(20)] // Giới hạn độ dài tối đa của RoomNumber
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "RoomNumber must be alphanumeric.")]
        public string RoomNumber { get; set; }

        public string Thumbnail { get; set; } = null!;
        public List<string> Images { get; set; } = [];// Có thể là một chuỗi JSON hoặc đường dẫn đến hình ảnh
        [Required] // Đảm bảo Price không được để trống
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public EStatusRoom Status { get; set; } = EStatusRoom.AVAILABLE;
        
        [JsonIgnore]
        public virtual RoomType RoomType { get; set; } = null!;
        [JsonIgnore]
        public virtual Hotel Hotel { get; set; } = null!;
    }
}
