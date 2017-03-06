using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Models;
using Demo.Core.Repositories;
using System.Linq.Dynamic;

namespace Demo.Core.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers(string orderByColumn);
        Task<Customer> GetCustomer(int customerId);
        Task<Customer> GetCustomerWithOrders(int customerId);
        Task<Customer> InsertCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
    }

    
    public class CustomerService : ICustomerService
    {
        private IReservationUnitOfWork _uow;

        public CustomerService(IReservationUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<Customer>> GetCustomers(string orderByColumn)
        {
            return _uow.CustomerRepository.Get(null, null, null);
        }

        public Task<Customer> GetCustomer(int customerId)
        {
            return _uow.CustomerRepository.GetById(customerId);
        }

        public Task<Customer> GetCustomerWithOrders(int customerId)
        {
            return _uow.CustomerRepository.GetCustomerWithOrders(customerId);
        }

        public async Task<Customer> InsertCustomer(Customer customer)
        {
            customer = _uow.CustomerRepository.Insert(customer);
            await _uow.Commit();
            return customer;

        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            customer = _uow.CustomerRepository.Update(customer);
            await _uow.Commit();
            return customer;
        }

        public void DeleteCustomer(int customerId)
        {
            _uow.CustomerRepository.Delete(customerId);
        }
    }
}
