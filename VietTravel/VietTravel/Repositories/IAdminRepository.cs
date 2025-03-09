using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public interface IAdminRepository
    {
        IEnumerable<Admin> GetAll(); // Lấy tất cả admin
        Admin GetById(string id); // Lấy admin theo ID
        void Add(Admin admin); // Thêm admin mới
        void Update(Admin admin); // Cập nhật admin
        void Delete(string id); // Xóa admin
    }
}