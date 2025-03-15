using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VietTravel.Models;


namespace VietTravel.Repositories
{
    public static class AdminRepositoryFactory
    {
        public static IAdminRepository CreateRepository()
        {
            return new AdminRepository(); // Trả về đối tượng cụ thể
        }
    }
}