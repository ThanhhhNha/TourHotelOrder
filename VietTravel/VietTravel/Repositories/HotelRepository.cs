using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly TravelVNEntities _context;

        public HotelRepository(TravelVNEntities context)
        {
            _context = context;
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _context.Hotels.Include(h => h.TinhThanh).ToList();
        }

        public Hotel GetHotelById(string id)
        {
            return _context.Hotels.Include(h => h.Phongs.Select(p => p.LoaiPhong))
                                  .Include(h => h.Phongs.Select(p => p.TrangThaiPhong))
                                  .FirstOrDefault(h => h.MaKhachSan == id);
        }

        public void AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
        }

        public void UpdateHotel(Hotel hotel)
        {
            _context.Entry(hotel).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteHotel(string id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
        }
    }
}
