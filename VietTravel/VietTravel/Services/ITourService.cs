using System;
using System.Collections.Generic;
using VietTravel.Models;

namespace VietTravel.Services
{
    public interface ITourService
    {
        List<Tour> GetAllTours();
        Tour GetTourById(string id);
        void AddTour(Tour tour);
        void UpdateTour(Tour tour);
        void DeleteTour(string id);
    }
}
