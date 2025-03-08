using System.Collections.Generic;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(string id);
        void Add(User user);
        void Update(User user);
        void Delete(string id);
    }
}
