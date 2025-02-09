using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LTBACKEND.Utils
{
    public class PaginationModel
    {
          [DefaultValue(1)]
        public int PageNumber { get; set; } = 1; // Trang hiện tại, mặc định là 1
         [DefaultValue(10)]
        public int PageSize { get; set; } = 10;  // Số lượng bản ghi mỗi trang, mặc định là 10
        public string? Search { get; set; }   // Từ khóa tìm kiếm tùy chọn
    }
}