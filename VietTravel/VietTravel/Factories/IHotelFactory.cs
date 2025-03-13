using VietTravel.Models;

namespace VietTravel.Factories
{
    public interface IHotelFactory
    {
        Hotel CreateHotel(string maKhachSan, string tenKhachSan, string diaChi, string moTa, string hinhAnh, string maTinh);
    }
}