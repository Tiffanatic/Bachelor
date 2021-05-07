using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;

namespace RapidTime.Services
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

        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            try
            {
                return _unitofWork.CustomerRepository.GetAll();

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

        public int Insert(CustomerEntity customerEntity)
        {
            try
            {
                
                var id =_unitofWork.CustomerRepository.Insert(customerEntity);
                _unitofWork.Commit();
                return id;

            }catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public void Update(CustomerEntity customerEntity)
        {
            try
            {
                _unitofWork.CustomerRepository.Update(customerEntity);
                _unitofWork.Commit();

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception(e.Message);
            }
        }

        public CustomerEntity GetById(int i)
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

        public CustomerEntity[] FindByName(string input)
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