using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class HomeController : Controller
    {
        private TravelVNEntities db = DatabaseConnection.Instance;

        public ActionResult Home()
        {
            var tinhList = db.TinhThanhs.ToList();

            if (!tinhList.Any())
            {
                return View("Error", new { message = "Không có dữ liệu tỉnh thành." });
            }

            ViewBag.TinhList = tinhList;
            return View();
        }

        public ActionResult Search(string MaTinh, DateTime? checkIn, DateTime? checkOut)
        {
            var tinhList = db.TinhThanhs.ToList();
            ViewBag.TinhList = tinhList;

            var selectedProvince = tinhList.FirstOrDefault(t => t.MaTinh == MaTinh);
            ViewBag.SelectedMaTinh = MaTinh;
            ViewBag.ProvinceName = selectedProvince?.TenTinh ?? "Chưa chọn tỉnh";

            ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd") ?? string.Empty;
            ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd") ?? string.Empty;

            var hotels = db.Hotels.Where(h => h.MaTinh == MaTinh).ToList();

            if (!hotels.Any())
            {
                ViewBag.NoHotelsMessage = "Không có khách sạn nào ở tỉnh thành này.";
                return View("ListHotels", new List<Hotel>());
            }

            return View("ListHotels", hotels);
        }

        public ActionResult SearchTour(string MaTinh, string Budget)
        {
            var tinhList = db.TinhThanhs.ToList();
            ViewBag.TinhList = tinhList;

            var selectedProvince = tinhList.FirstOrDefault(t => t.MaTinh == MaTinh);
            ViewBag.SelectedMaTinh = MaTinh;
            ViewBag.ProvinceName = selectedProvince?.TenTinh ?? "Chưa chọn tỉnh";

            var tours = db.Tours.Where(t => t.MaTinh == MaTinh);

            // Sử dụng if-else thay vì switch để tránh lỗi khi sử dụng tính năng không hỗ trợ
            if (!string.IsNullOrEmpty(Budget))
            {
                if (Budget == "under-5m")
                {
                    tours = tours.Where(t => t.Gia < 5000000);
                }
                else if (Budget == "5-7m")
                {
                    tours = tours.Where(t => t.Gia >= 5000000 && t.Gia <= 7000000);
                }
                else if (Budget == "above-7m")
                {
                    tours = tours.Where(t => t.Gia > 7000000);
                }
            }

            var tourList = tours.ToList();

            if (!tourList.Any())
            {
                ViewBag.NoToursMessage = "Không có tour nào ở tỉnh thành này.";
                return View("ListTours", new List<Tour>());
            }

            return View("ListTours", tourList);
        }

        public IEnumerable<Hotel> GetHotelsByMaTinh(string MaTinh)
        {
            return db.Hotels.Where(h => h.MaTinh == MaTinh).ToList();
        }
    }

    // Singleton Class to manage database connection
    public class DatabaseConnection
    {
        private static TravelVNEntities _instance;

        private DatabaseConnection() { }

        public static TravelVNEntities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TravelVNEntities();
                }
                return _instance;
            }
        }
    }
}
