using System.Collections.Generic;
using System.Linq;
using VietTravel.Models;

namespace VietTravel.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TravelVNEntities _context;

        public UserRepository()
        {
            _context = new TravelVNEntities();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
