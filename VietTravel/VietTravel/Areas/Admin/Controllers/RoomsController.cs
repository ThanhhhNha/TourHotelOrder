using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;
using VietTravel.Models.RoomProcessing;

namespace VietTravel.Areas.Admin.Controllers
{
    public class RoomsController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/Rooms
        public ActionResult Index()
        {
            var phongs = db.Phongs.Include(p => p.Hotel).Include(p => p.LoaiPhong).Include(p => p.TrangThaiPhong).Include(p => p.User);
            return View(phongs.ToList());
        }

        // GET: Admin/Rooms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // GET: Admin/Rooms/Create
        public ActionResult Create()
        {
            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan");
            ViewBag.MaLoai = new SelectList(db.LoaiPhongs, "MaLoai", "TenLoai");
            ViewBag.MaTrangThai = new SelectList(db.TrangThaiPhongs, "MaTrangThai", "TenTrangThai");
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "TenUser");
            return View();
        }

        // POST: Admin/Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Phong phong, HttpPostedFileBase Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null && Hinh.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Hinh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Images/"), fileName);
                    Hinh.SaveAs(path);
                    phong.Hinh = "/Uploads/Images/" + fileName;
                }

                RoomProcessor processor = new CreateRoomProcessor();
                processor.Process(phong); // Áp dụng Template Method

                return RedirectToAction("Index");
            }

            return View(phong);
        }


        // GET: Admin/Rooms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachSan = new SelectList(db.Hotels, "MaKhachSan", "TenKhachSan", phong.MaKhachSan);
            ViewBag.MaLoai = new SelectList(db.LoaiPhongs, "MaLoai", "TenLoai", phong.MaLoai);
            ViewBag.MaTrangThai = new SelectList(db.TrangThaiPhongs, "MaTrangThai", "TenTrangThai", phong.MaTrangThai);
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "TenUser", phong.MaUser);
            return View(phong);
        }

        // POST: Admin/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Phong phong)
        {
            if (ModelState.IsValid)
            {
                RoomProcessor processor = new EditRoomProcessor();
                processor.Process(phong); // Áp dụng Template Method

                return RedirectToAction("Index");
            }

            return View(phong);
        }

        // GET: Admin/Rooms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Admin/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Phong phong = db.Phongs.Find(id);
            RoomProcessor processor = new DeleteRoomProcessor();
            processor.Process(phong); // Áp dụng Template Method

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
