using VietTravel.Models;

namespace VietTravel.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelVNEntities _context;
        private IGenericRepository<OrderBooking> _orderBookingRepository;
        private IGenericRepository<Hotel> _hotelRepository;

        public UnitOfWork(TravelVNEntities context)
        {
            _context = context;
        }

        public IGenericRepository<OrderBooking> OrderBookingRepository
        {
            get
            {
                if (_orderBookingRepository == null)
                {
                    _orderBookingRepository = new GenericRepository<OrderBooking>(_context);
                }
                return _orderBookingRepository;
            }
        }


        public IGenericRepository<Hotel> HotelRepository
        {
            get
            {
                if (_hotelRepository == null)
                {
                    _hotelRepository = new GenericRepository<Hotel>(_context);
                }
                return _hotelRepository;
            }
        }


        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
