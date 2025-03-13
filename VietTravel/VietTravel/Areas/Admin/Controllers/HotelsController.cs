using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class HotelsController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/Hotels
        public ActionResult Index()
        {
            var hotels = db.Hotels.Include(h => h.TinhThanh);
            return View(hotels.ToList());
        }

        // GET: Admin/Hotels/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Admin/Hotels/Create
        public ActionResult Create()
        {
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh");
            return View();
        }

        // POST: Admin/Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachSan,TenKhachSan,DiaChi,MoTa,MaTinh")] Hotel hotel, HttpPostedFileBase Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null && Hinh.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Hinh.FileName);
                    var path = Path.Combine(Server.MapPath("~/image/hotel/"), fileName);

                    Hinh.SaveAs(path);
                    hotel.HinhAnh = "~/image/hotel/" + fileName;
                }

                try
                {
                    db.Hotels.Add(hotel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", validationError.ErrorMessage);
                        }
                    }
                }
            }

            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
            return View(hotel);
        }



        // GET: Admin/Hotels/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
            return View(hotel);
        }

        // POST: Admin/Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachSan,TenKhachSan,DiaChi,MoTa,HinhAnh,MaTinh")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", hotel.MaTinh);
            return View(hotel);
        }

        // GET: Admin/Hotels/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Admin/Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
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
