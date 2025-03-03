using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VietTravel.Models;

namespace VietTravel.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/Account
        public ActionResult AdminLogin()
        {
            return View();
        }


        public ActionResult AdminHome()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin", "Account", new { area = "Admin" });
        }

        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
            var admin = db.Admins.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Passwords == password);

            if (admin != null)
            {
                Session["user"] = admin;
                Session["userName"] = admin.Username;
                return RedirectToAction("AdminHome", "Account", new { area = "Admin" });
            }
            else
            {
                TempData["error"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return RedirectToAction("AdminLogin", "Account", new { area = "Admin" });
            }
        }
        public ActionResult AdminAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VietTravel.Models.Admin admin = db.Admins.SingleOrDefault(u => u.MaAdmin == id);

            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }
        public ActionResult AdminEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VietTravel.Models.Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AdminEdit([Bind(Include = "MaAdmin,TenAdmin,VaiTro,Username,Passwords")] Admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(admin).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("AdminAccount", "Account", new { id = admin.MaAdmin });
        //    }
        //    return View(admin);
        //}



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

