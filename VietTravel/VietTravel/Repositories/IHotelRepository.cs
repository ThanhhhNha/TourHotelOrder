using System.Collections.Generic;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public interface IHotelRepository
    {
        IEnumerable<Hotel> GetAllHotels();
        Hotel GetHotelById(string id);
        void AddHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(string id);
    }
}