using System;
using System.Net;
using System.Web.Mvc;
using VietTravel.Models;
using VietTravel.Services;
using System.Linq;

namespace VietTravel.Controllers
{
    public class ToursController : Controller
    {
        private readonly ITourService _tourService;

        public ToursController()
        {
            _tourService = new TourService(new TravelVNEntities());
        }

        public ActionResult ListTours()
        {
            var tours = _tourService.GetAllTours();
            return View(tours);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tour = _tourService.GetTourById(id);
            if (tour == null)
            {
                return HttpNotFound();
            }

            return View(tour);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                _tourService.AddTour(tour);
                return RedirectToAction("ListTours");
            }
            return View(tour);
        }

        public ActionResult Edit(string id)
        {
            var tour = _tourService.GetTourById(id);
            if (tour == null)
            {
                return HttpNotFound();
            }

            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                _tourService.UpdateTour(tour);
                return RedirectToAction("ListTours");
            }
            return View(tour);
        }

        public ActionResult Delete(string id)
        {
            var tour = _tourService.GetTourById(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _tourService.DeleteTour(id);
            return RedirectToAction("ListTours");
        }
    }
}
