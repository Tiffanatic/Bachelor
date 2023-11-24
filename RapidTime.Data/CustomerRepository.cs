using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RapidTime.Core.Models;
using RapidTime.Core.Repositories;
using System.Linq;

namespace RapidTime.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RapidTimeDbContext _context;

        public CustomerRepository(RapidTimeDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerEntity> GetAll()
        {
            return _context.Customers.Include(e => e.CompanyTypeEntity);
        }

        public CustomerEntity Insert(CustomerEntity customerEntity)
        {
            if (customerEntity is null) throw new ArgumentNullException("customerEntity");
            customerEntity.Created = DateTime.UtcNow;
            customerEntity.Updated = DateTime.UtcNow;
            var entityEntry = _context.Customers.Add(customerEntity);
            return entityEntry.Entity;
        }

        public CustomerEntity GetbyId(int id)
        {
            return _context.Customers.SingleOrDefault(s => s.Id == id);
        }

        public void Update(CustomerEntity customerEntity)
        {
            if ( customerEntity == null) throw new ArgumentNullException("customerEntity");
            customerEntity.Updated = DateTime.UtcNow;
            _context.Customers.Update(customerEntity);
        }

        public void Delete(int id)
        {
            CustomerEntity entity = _context.Customers.SingleOrDefault(s => s.Id == id);
            if (entity == null)
            {
                throw new ArgumentException("Item was not found");
            }
            _context.Customers.Remove(entity);
        }
    }
}