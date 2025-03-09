using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private TravelVNEntities db = new TravelVNEntities();

        public IEnumerable<Admin> GetAll()
        {
            return db.Admins.ToList(); // Lấy danh sách admin
        }

        public Admin GetById(string id)
        {
            return db.Admins.Find(id); // Tìm admin theo ID
        }

        public void Add(Admin admin)
        {
            db.Admins.Add(admin);
            db.SaveChanges(); // Lưu vào DB
        }

        public void Update(Admin admin)
        {
            db.Entry(admin).State = EntityState.Modified;
            db.SaveChanges(); // Cập nhật DB
        }

        public void Delete(string id)
        {
            var admin = db.Admins.Find(id);
            if (admin != null)
            {
                db.Admins.Remove(admin);
                db.SaveChanges(); // Xóa khỏi DB
            }
        }
    }
}