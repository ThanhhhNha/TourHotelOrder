using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietTravel.Models
{
    public class NullHotel : Hotel
    {
        public NullHotel()
        {
            MaKhachSan = "N/A";
            TenKhachSan = "Thông tin không có";
            DiaChi = "Chưa có địa chỉ";
            MoTa = "Không có mô tả";
            HinhAnh = "~/image/hotel/default.jpg"; // Ảnh mặc định nếu không có hình
            MaTinh = "N/A";
            TinhThanh = new TinhThanh { MaTinh = "N/A", TenTinh = "Không rõ" };
        }
    }
}
