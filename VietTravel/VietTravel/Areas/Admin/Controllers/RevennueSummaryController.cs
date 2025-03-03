using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VietTravel.Models;
using System.Data.Entity;

namespace VietTravel.Areas.Admin.Controllers
{
    public class RevennueSummaryController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/RevenueSummary
        public ActionResult RevenueSummary()
        {
            var today = DateTime.Now.Date;

            // Lấy tất cả đơn đặt phòng của hôm nay
            var bookings = db.OrderBookings
                             .Where(b => DbFunctions.TruncateTime(b.BookingDate) == today)
                             .ToList();

            // Tính tổng số phòng đã đặt
            var totalRoomsBooked = bookings.Sum(b => b.RoomQuantity);

            // Tính tổng doanh thu từ phòng đã đặt
            var totalRevenue = bookings.Sum(b => (b.Price ?? 0) * b.RoomQuantity); 

            // Tạo đối tượng RevenueSummary
            var revenueSummary = new RevenueSummary
            {
                TotalRoomsBooked = totalRoomsBooked,
                TotalRevenue = totalRevenue,
                Date = today
            };

            return View(revenueSummary);
        }

        // GET: Admin/RevenueTourSummary
        public ActionResult RevenueTourSummary()
        {
            var today = DateTime.Now.Date;

            // Lấy tất cả đơn đặt tour của hôm nay
            var tourBookings = db.TourBookings
                                 .Where(b => DbFunctions.TruncateTime(b.BookingDate) == today)
                                 .ToList();

            // Tính tổng số tour đã đặt
            var totalToursBooked = tourBookings.Count;

            // Tính tổng doanh thu từ tour đã đặt
            var totalRevenue = tourBookings.Sum(b => b.Tour.Gia);

            // Tạo đối tượng RevenueSummary
            var revenueSummary = new RevenueSummary
            {
                TotalRoomsBooked = totalToursBooked, 
                TotalRevenue = totalRevenue,
                Date = today
            };

            return View(revenueSummary);
        }
    }
}
