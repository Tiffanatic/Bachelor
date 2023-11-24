using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitofWork unitofWork, ILogger<CustomerService> logger)
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
                _logger.LogInformation("Error in CustomerService.GetAllCustomers: {Error}", e.Message);
                throw new Exception(e.Message);
            }
            
        }


        public void Delete(int customerId)
        {
            try
            {
                _unitofWork.CustomerRepository.Delete(customerId);
                _unitofWork.Commit();

            }
            catch (Exception e)
            {
                _logger.LogInformation("Error in CustomerService.Delete: {Error}", e.Message);
                throw new Exception(e.Message);
            }
        }

        public int Insert(CustomerEntity customerEntity)
        {
            try
            {
                
                var id =_unitofWork.CustomerRepository.Insert(customerEntity);
                _unitofWork.Commit();

                return id.Id;

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

        // Not used as the gRPC service only implements GetById(int i)
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