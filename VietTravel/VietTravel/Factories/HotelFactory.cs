using VietTravel.Models;

namespace VietTravel.Factories
{
    public class HotelFactory : IHotelFactory
    {
        public Hotel CreateHotel(string maKhachSan, string tenKhachSan, string diaChi, string moTa, string hinhAnh, string maTinh)
        {
            return new Hotel
            {
                MaKhachSan = maKhachSan,
                TenKhachSan = tenKhachSan,
                DiaChi = diaChi,
                MoTa = moTa,
                HinhAnh = hinhAnh,
                MaTinh = maTinh
            };
        }
    }
}
