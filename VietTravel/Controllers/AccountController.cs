using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class AccountController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/DangNhap
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không được để trống";
                return View();
            }

            // Tìm user trong cơ sở dữ liệu
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Đăng nhập thành công - Lưu thông tin user vào Session
                Session["Account"] = user;

                // Điều hướng về trang chủ hoặc trang phù hợp
                return RedirectToAction("Home", "Home");
            }
            else
            {
                // Đăng nhập thất bại
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

        // GET: Account/DangXuat
        public ActionResult Logout()
        {
            // Xóa session khi người dùng đăng xuất
            Session["Account"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string tenUser, string username, string password, string email, string dienThoai)
        {
            if (string.IsNullOrWhiteSpace(tenUser) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                TempData["error"] = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }

            if (dienThoai.Length != 10)
            {
                TempData["error"] = "Số điện thoại phải có đúng 10 chữ số!";
                return View();
            }

            var existingUser = db.Users.SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (existingUser != null)
            {
                TempData["error"] = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Tạo mã người dùng duy nhất
            string maUser = "U" + Guid.NewGuid().ToString("N").Substring(0, 9).ToUpper();

            User newUser = new User
            {
                MaUser = maUser,
                TenUser = tenUser,
                Username = username,
                Password = password,
                Email = email,
                DienThoai = dienThoai
            };

            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();

                TempData["success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
                return RedirectToAction("Login");
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("<br/>", ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage));
                TempData["error"] = "Lỗi xác thực: " + errorMessages;
                return View();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["error"] = "Có lỗi xảy ra trong quá trình đăng ký: " + innerMessage;
                return View();
            }
        }
        public ActionResult Deleted(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaUser,TenUser,Username,Password,Email,DienThoai")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Account", new { id = user.MaUser });
            }
            return View(user);
        }


        public ActionResult Account(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            User user = db.Users.SingleOrDefault(u => u.MaUser == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
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