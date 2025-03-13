using System;
using VietTravel.Models;

namespace VietTravel.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<OrderBooking> OrderBookingRepository { get; }
        IGenericRepository<Hotel> HotelRepository { get; }
        void Commit();
    }
}
