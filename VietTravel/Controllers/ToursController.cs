using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class ToursController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Tours
        public ActionResult ListTours(string price, string departurePoint, string destination, DateTime? departureDate, string transport)
        {
            var tours = db.Tours
                .Include(t => t.LoaiTour)
                .Include(t => t.TinhThanh)
                .AsQueryable();

            // Price filter
            if (!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "Under3M":
                        tours = tours.Where(t => t.Gia < 3000000);
                        break;
                    case "3to5M":
                        tours = tours.Where(t => t.Gia >= 3000000 && t.Gia <= 5000000);
                        break;
                    case "5to7M":
                        tours = tours.Where(t => t.Gia > 5000000 && t.Gia <= 7000000);
                        break;
                    case "Above7M":
                        tours = tours.Where(t => t.Gia > 7000000);
                        break;
                }
            }

            // Departure point filter
            if (!string.IsNullOrEmpty(departurePoint))
            {
                tours = tours.Where(t => t.TinhThanh != null && t.TinhThanh.TenTinh == departurePoint);
            }

            // Transport filter
            if (!string.IsNullOrEmpty(transport))
            {
                tours = tours.Where(t => t.PhuongTien == transport);
            }

            // Departure date filter
            if (departureDate.HasValue)
            {
                DateTime dateOnly = departureDate.Value.Date;  // Use just the Date part
                tours = tours.Where(t => DbFunctions.TruncateTime(t.NgayKhoiHanh) == dateOnly);
            }

            return View(tours.ToList());
        }


        // GET: Tours/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour");
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh");
            return View();
        }

        // POST: Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // GET: Tours/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
