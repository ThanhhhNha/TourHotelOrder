using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Areas.Admin.Controllers
{
    public class TourBookingsController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/TourBookings
        public ActionResult Index()
        {
            var tourBookings = db.TourBookings.Include(t => t.Tour);
            return View(tourBookings.ToList());
        }

        // GET: Admin/TourBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourBooking tourBooking = db.TourBookings.Find(id);
            if (tourBooking == null)
            {
                return HttpNotFound();
            }
            return View(tourBooking);
        }

        // GET: Admin/TourBookings/Create
        public ActionResult Create()
        {
            ViewBag.TourId = new SelectList(db.Tours, "MaTour", "TenTour");
            return View();
        }

        // POST: Admin/TourBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,Name,Email,Phone,Title,FullName,Dob,Requests,PaymentMethod,TourId")] TourBooking tourBooking)
        {
            if (ModelState.IsValid)
            {
                db.TourBookings.Add(tourBooking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TourId = new SelectList(db.Tours, "MaTour", "TenTour", tourBooking.TourId);
            return View(tourBooking);
        }

        // GET: Admin/TourBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourBooking tourBooking = db.TourBookings.Find(id);
            if (tourBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.TourId = new SelectList(db.Tours, "MaTour", "TenTour", tourBooking.TourId);
            return View(tourBooking);
        }

        // POST: Admin/TourBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,Name,Email,Phone,Title,FullName,Dob,Requests,PaymentMethod,TourId")] TourBooking tourBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tourBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TourId = new SelectList(db.Tours, "MaTour", "TenTour", tourBooking.TourId);
            return View(tourBooking);
        }

        // GET: Admin/TourBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourBooking tourBooking = db.TourBookings.Find(id);
            if (tourBooking == null)
            {
                return HttpNotFound();
            }
            return View(tourBooking);
        }

        // POST: Admin/TourBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TourBooking tourBooking = db.TourBookings.Find(id);
            db.TourBookings.Remove(tourBooking);
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
