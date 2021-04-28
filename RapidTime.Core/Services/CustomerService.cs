using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitofWork _unitofWork;
        private ILogger _logger;

        public CustomerService(IUnitofWork unitofWork, ILogger logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            try
            {
                return _unitofWork.CustomerRepository.getAll();

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
            
        }


        public void Delete(int customerId)
        {
            if (customerId == null) throw new NullReferenceException();
            try
            {
                _unitofWork.CustomerRepository.Delete(customerId);
                _unitofWork.Commit();

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public void Insert(Customer customer)
        {
            try
            {
            _unitofWork.CustomerRepository.Insert(customer);
            _unitofWork.Commit();
                
            }catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public void Update(Customer customer)
        {
            try
            {
                _unitofWork.CustomerRepository.Update(customer);
                _unitofWork.Commit();

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public Customer GetById(int i)
        {
            try
            {
                return _unitofWork.CustomerRepository.GetbyId(i);

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public Customer[] FindByName(string input)
        {
            try
            {
                var customers = GetAllCustomers();
                return customers.Where(x => x.Name.Contains(input)).ToArray();
                
            }catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}