using System;
using System.Linq;
using VietTravel.Models;
using System.Data.Entity;

namespace VietTravel.Services
{
    public class RevenueService
    {
        private readonly TravelVNEntities _db;

        public RevenueService(TravelVNEntities db)
        {
            _db = db;
        }

        public RevenueSummary GetRoomRevenueSummary(DateTime date)
        {
            var bookings = _db.OrderBookings
                             .Where(b => DbFunctions.TruncateTime(b.BookingDate) == date)
                             .ToList();

            return new RevenueSummary
            {
                TotalRoomsBooked = bookings.Sum(b => b.RoomQuantity),
                TotalRevenue = bookings.Sum(b => (b.Price ?? 0) * b.RoomQuantity),
                Date = date
            };
        }

        public RevenueSummary GetTourRevenueSummary(DateTime date)
        {
            var tourBookings = _db.TourBookings
                                 .Where(b => DbFunctions.TruncateTime(b.BookingDate) == date)
                                 .ToList();

            return new RevenueSummary
            {
                TotalRoomsBooked = tourBookings.Count,
                TotalRevenue = tourBookings.Sum(b => b.Tour.Gia),
                Date = date
            };
        }
    }
}
