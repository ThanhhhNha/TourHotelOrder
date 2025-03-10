using System;
using System.Collections.Generic;
using System.Linq;
using VietTravel.Models;

namespace VietTravel.Services
{
    public class TourService : ITourService
    {
        private readonly TravelVNEntities _context;

        public TourService(TravelVNEntities context)
        {
            _context = context;
        }

        public List<Tour> GetAllTours()
        {
            return _context.Tours.Include("LoaiTour").Include("TinhThanh").ToList();
        }

        public Tour GetTourById(string id)
        {
            return _context.Tours.FirstOrDefault(t => t.MaTour == id);
        }

        public void AddTour(Tour tour)
        {
            _context.Tours.Add(tour);
            _context.SaveChanges();
        }

        public void UpdateTour(Tour tour)
        {
            _context.Entry(tour).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteTour(string id)
        {
            var tour = _context.Tours.Find(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                _context.SaveChanges();
            }
        }
    }
}
