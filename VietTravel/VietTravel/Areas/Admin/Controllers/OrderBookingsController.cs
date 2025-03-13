using System.Linq;
using System.Net;
using System.Web.Mvc;
using VietTravel.DataAccess;
using VietTravel.Models;

namespace VietTravel.Areas.Admin.Controllers
{
    public class OrderBookingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderBookingsController()
        {
            _unitOfWork = new UnitOfWork(new TravelVNEntities());
        }

        public ActionResult Index()
        {
            var orderBookings = _unitOfWork.OrderBookingRepository.GetAll(includeProperties: "Hotel");
            return View(orderBookings);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orderBooking = _unitOfWork.OrderBookingRepository.GetById(id);
            if (orderBooking == null) return HttpNotFound();

            return View(orderBooking);
        }

        public ActionResult Create()
        {
            ViewBag.MaKhachSan = new SelectList(_unitOfWork.HotelRepository.GetAll(), "MaKhachSan", "TenKhachSan");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone,Title,FullName,Dob,Requests,RoomType,Price,CheckinDate,CheckoutDate,RoomQuantity,BookingDate,MaKhachSan")] OrderBooking orderBooking)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderBookingRepository.Add(orderBooking);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.MaKhachSan = new SelectList(_unitOfWork.HotelRepository.GetAll(), "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
            return View(orderBooking);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orderBooking = _unitOfWork.OrderBookingRepository.GetById(id);
            if (orderBooking == null) return HttpNotFound();

            ViewBag.MaKhachSan = new SelectList(_unitOfWork.HotelRepository.GetAll(), "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
            return View(orderBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,Title,FullName,Dob,Requests,RoomType,Price,CheckinDate,CheckoutDate,RoomQuantity,BookingDate,MaKhachSan")] OrderBooking orderBooking)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderBookingRepository.Update(orderBooking);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.MaKhachSan = new SelectList(_unitOfWork.HotelRepository.GetAll(), "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
            return View(orderBooking);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orderBooking = _unitOfWork.OrderBookingRepository.GetById(id);
            if (orderBooking == null) return HttpNotFound();

            return View(orderBooking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.OrderBookingRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using VietTravel.Models;

//namespace VietTravel.Areas.Admin.Controllers
//{
//    public class OrderBookingsController : Controller
//    {
//        private TravelVNEntities db = new TravelVNEntities();

//        // GET: Admin/OrderBookings1
//        public ActionResult Index()
//        {
//            var orderBookings = db.OrderBookings.Include(o => o.Hotel);
//            return View(orderBookings.ToList());
//        }

//        // GET: Admin/OrderBookings1/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OrderBooking orderBooking = db.OrderBookings.Find(id);
//            if (orderBooking == null)
//            {
//                return HttpNotFound();
//            }
//            return View(orderBooking);
//        }

//        // GET: Admin/OrderBookings1/Create
//        public ActionResult Create()
//        {
//            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan");
//            return View();
//        }

//        // POST: Admin/OrderBookings1/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone,Title,FullName,Dob,Requests,RoomType,Price,CheckinDate,CheckoutDate,RoomQuantity,BookingDate,MaKhachSan")] OrderBooking orderBooking)
//        {
//            if (ModelState.IsValid)
//            {
//                db.OrderBookings.Add(orderBooking);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
//            return View(orderBooking);
//        }

//        // GET: Admin/OrderBookings1/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OrderBooking orderBooking = db.OrderBookings.Find(id);
//            if (orderBooking == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
//            return View(orderBooking);
//        }

//        // POST: Admin/OrderBookings1/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,Title,FullName,Dob,Requests,RoomType,Price,CheckinDate,CheckoutDate,RoomQuantity,BookingDate,MaKhachSan")] OrderBooking orderBooking)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(orderBooking).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan", orderBooking.MaKhachSan);
//            return View(orderBooking);
//        }

//        // GET: Admin/OrderBookings1/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OrderBooking orderBooking = db.OrderBookings.Find(id);
//            if (orderBooking == null)
//            {
//                return HttpNotFound();
//            }
//            return View(orderBooking);
//        }

//        // POST: Admin/OrderBookings1/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            OrderBooking orderBooking = db.OrderBookings.Find(id);
//            db.OrderBookings.Remove(orderBooking);
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
