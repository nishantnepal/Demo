using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Models;

namespace Demo.Core.Repositories
{
    public class ReservationUnitOfWork : IDisposable, IReservationUnitOfWork
    {
        private readonly ReservationContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        //public ReservationUnitOfWork()
        //{
        //    _context = new ReservationContext();
        //    _customerRepository = new CustomerRepository(_context);
        //    _orderRepository = new OrderRepository(_context);
        //}

        public ReservationUnitOfWork(ReservationContext context, ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public ICustomerRepository CustomerRepository => _customerRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public async Task Commit()
        {
           await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
