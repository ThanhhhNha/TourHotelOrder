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
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;
//using VietTravel.Models;

//namespace VietTravel.Controllers
//{
//    public class ListHotelsController : Controller
//    {
//        private TravelVNEntities db = new TravelVNEntities();
//        public ActionResult ListHotelsAll(string SearchString)
//        {
//            var hotels = from h in db.Hotels select h;

//            if (!String.IsNullOrEmpty(SearchString))
//            {
//                hotels = hotels.Where(h => h.MaTinh.Contains(SearchString) || h.MaTinh.Contains(SearchString));
//            }

//            return View(hotels.ToList());
//        }


//        // GET: ListHotels
//        public ActionResult ListHotels()
//        {
//            var hotels = db.Hotels.Include(h => h.TinhThanh);
//            return View(hotels.ToList());
//        }

//        // GET: ListHotels/Details/5
//        public ActionResult HotelsDetail(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }


//            Hotel hotel = db.Hotels
//                            .Include(h => h.Phongs.Select(p => p.LoaiPhong))
//                            .Include(h => h.Phongs.Select(p => p.TrangThaiPhong))
//                            .FirstOrDefault(h => h.MaKhachSan == id);

//            if (hotel == null)
//            {
//                return HttpNotFound();
//            }

//            return View(hotel);
//        }


//        // GET: ListHotels/Create
//        public ActionResult Create()
//        {
//            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh");
//            return View();
//        }

//        // POST: ListHotels/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "MaKhachSan,TenKhachSan,DiaChi,MoTa,HinhAnh,MaTinh")] Hotel hotel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Hotels.Add(hotel);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
//            return View(hotel);
//        }

//        // GET: ListHotels/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Hotel hotel = db.Hotels.Find(id);
//            if (hotel == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
//            return View(hotel);
//        }

//        // POST: ListHotels/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "MaKhachSan,TenKhachSan,DiaChi,MoTa,HinhAnh,MaTinh")] Hotel hotel)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(hotel).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
//            return View(hotel);
//        }

//        // GET: ListHotels/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Hotel hotel = db.Hotels.Find(id);
//            if (hotel == null)
//            {
//                return HttpNotFound();
//            }
//            return View(hotel);
//        }

//        // POST: ListHotels/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            Hotel hotel = db.Hotels.Find(id);
//            db.Hotels.Remove(hotel);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}