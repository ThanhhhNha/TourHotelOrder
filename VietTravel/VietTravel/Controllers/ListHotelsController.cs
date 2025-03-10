using System.Net;
using System.Web.Mvc;
using VietTravel.Models;
using VietTravel.Repositories;
using VietTravel.Factories;
using System.Linq;

namespace VietTravel.Controllers
{
    public class ListHotelsController : Controller
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelFactory _hotelFactory;

        public ListHotelsController()
        {
            var context = new TravelVNEntities();
            _hotelRepository = new HotelRepository(context);
            _hotelFactory = new HotelFactory();
        }

        public ActionResult ListHotelsAll(string SearchString)
        {
            var hotels = _hotelRepository.GetAllHotels();

            if (!string.IsNullOrEmpty(SearchString))
            {
                hotels = hotels.Where(h => h.MaTinh.Contains(SearchString));
            }

            return View(hotels.ToList());
        }

        public ActionResult ListHotels()
        {
            var hotels = _hotelRepository.GetAllHotels();
            return View(hotels.ToList());
        }

        public ActionResult HotelsDetail(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Hotel hotel = _hotelRepository.GetHotelById(id);
            if (hotel == null) return HttpNotFound();

            return View(hotel);
        }

        public ActionResult Create()
        {
            ViewBag.MaTinh = new SelectList(new TravelVNEntities().TinhThanhs, "MaTinh", "TenTinh");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string maKhachSan, string tenKhachSan, string diaChi, string moTa, string hinhAnh, string maTinh)
        {
            if (ModelState.IsValid)
            {
                Hotel hotel = _hotelFactory.CreateHotel(maKhachSan, tenKhachSan, diaChi, moTa, hinhAnh, maTinh);
                _hotelRepository.AddHotel(hotel);
                return RedirectToAction("ListHotels");
            }

            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Hotel hotel = _hotelRepository.GetHotelById(id);
            if (hotel == null) return HttpNotFound();

            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _hotelRepository.UpdateHotel(hotel);
                return RedirectToAction("ListHotels");
            }

            return View(hotel);
        }

        public ActionResult Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Hotel hotel = _hotelRepository.GetHotelById(id);
            if (hotel == null) return HttpNotFound();

            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _hotelRepository.DeleteHotel(id);
            return RedirectToAction("ListHotels");
        }
    }
}
