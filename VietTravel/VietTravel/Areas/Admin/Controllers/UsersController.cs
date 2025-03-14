using System.Net;
using System.Web.Mvc;
using VietTravel.Filters;
using VietTravel.Models;
using VietTravel.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace VietTravel.Areas.Admin.Controllers
{
    [AdminAuthorize]
    ///Repository Pattern : Tách biệt giữa logic truy cập dữ liệu và Controller.Dễ dàng test hơn vì có thể mock IUserRepository. Dễ dàng bảo trì và mở rộng hệ thống.
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        // Inject IUserRepository thông qua constructor
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _userRepository.GetById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Add(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _userRepository.GetById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _userRepository.GetById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _userRepository.Delete(id);
            return RedirectToAction("Index");
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
//    public class UsersController : Controller
//    {
//        private TravelVNEntities db = new TravelVNEntities();

//        // GET: Admin/Users
//        public ActionResult Index()
//        {
//            return View(db.Users.ToList());
//        }

//        // GET: Admin/Users/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        // GET: Admin/Users/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Admin/Users/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "MaUser,TenUser,Username,Password,Email,DienThoai")] User user)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Users.Add(user);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(user);
//        }

//        // GET: Admin/Users/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        // POST: Admin/Users/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "MaUser,TenUser,Username,Password,Email,DienThoai")] User user)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(user).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(user);
//        }

//        // GET: Admin/Users/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        // POST: Admin/Users/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            User user = db.Users.Find(id);
//            db.Users.Remove(user);
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
