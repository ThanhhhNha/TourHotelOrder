using System;
using System.Diagnostics;
using System.Web.Mvc;
using VietTravel.Models;
using VietTravel.Services;

namespace VietTravel.Areas.Admin.Controllers
{
    //Facade Pattern: Controller nhẹ hơn: Không chứa logic tính toán dữ liệu.
    //Tách biệt trách nhiệm: Controller lo việc điều hướng, còn RevenueService xử lý dữ liệu.
    //Dễ bảo trì & mở rộng: Nếu cần thay đổi cách tính doanh thu, chỉ cần sửa RevenueService
    public class RevennueSummaryController : Controller
    {
        private readonly RevenueService _revenueService;

        public RevennueSummaryController()
        {
            _revenueService = new RevenueService(new TravelVNEntities());
        }

        public ActionResult RevenueSummary()
        {
            var today = DateTime.Now.Date;
            var revenueSummary = _revenueService.GetRoomRevenueSummary(today);
            return View(revenueSummary);
        }

        public ActionResult RevenueTourSummary()
        {
            var today = DateTime.Now.Date;
            var revenueSummary = _revenueService.GetTourRevenueSummary(today);
            return View(revenueSummary);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using VietTravel.Models;
//using System.Data.Entity;

//namespace VietTravel.Areas.Admin.Controllers
//{
//    public class RevennueSummaryController : Controller
//    {
//        private TravelVNEntities db = new TravelVNEntities();

//        // GET: Admin/RevenueSummary
//        public ActionResult RevenueSummary()
//        {
//            var today = DateTime.Now.Date;

//            // Lấy tất cả đơn đặt phòng của hôm nay
//            var bookings = db.OrderBookings
//                             .Where(b => DbFunctions.TruncateTime(b.BookingDate) == today)
//                             .ToList();

//            // Tính tổng số phòng đã đặt
//            var totalRoomsBooked = bookings.Sum(b => b.RoomQuantity);

//            // Tính tổng doanh thu từ phòng đã đặt
//            var totalRevenue = bookings.Sum(b => (b.Price ?? 0) * b.RoomQuantity); 

//            // Tạo đối tượng RevenueSummary
//            var revenueSummary = new RevenueSummary
//            {
//                TotalRoomsBooked = totalRoomsBooked,
//                TotalRevenue = totalRevenue,
//                Date = today
//            };

//            return View(revenueSummary);
//        }

//        // GET: Admin/RevenueTourSummary
//        public ActionResult RevenueTourSummary()
//        {
//            var today = DateTime.Now.Date;

//            // Lấy tất cả đơn đặt tour của hôm nay
//            var tourBookings = db.TourBookings
//                                 .Where(b => DbFunctions.TruncateTime(b.BookingDate) == today)
//                                 .ToList();

//            // Tính tổng số tour đã đặt
//            var totalToursBooked = tourBookings.Count;

//            // Tính tổng doanh thu từ tour đã đặt
//            var totalRevenue = tourBookings.Sum(b => b.Tour.Gia);

//            // Tạo đối tượng RevenueSummary
//            var revenueSummary = new RevenueSummary
//            {
//                TotalRoomsBooked = totalToursBooked, 
//                TotalRevenue = totalRevenue,
//                Date = today
//            };

//            return View(revenueSummary);
//        }
//    }
//}
