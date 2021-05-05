using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class PriceService : IPriceService
    {
        private readonly IUnitofWork _unitofWork;
        private ILogger _logger;

        public PriceService(IUnitofWork unitofWork, ILogger logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public Price GetById(int i)
        {
            return _unitofWork.PriceRepository.GetbyId(i);
        }

        public IEnumerable<Price> GetAll()
        {
            return _unitofWork.PriceRepository.GetAll();
        }

        public void Insert(Price price)
        {
            try
            {
                _unitofWork.PriceRepository.Insert(price);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public void Update(Price price)
        {
            try
            {
                _unitofWork.PriceRepository.Update(price);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public void Delete(int priceId)
        {
            if (priceId == null) throw new ArgumentNullException();
            try
            {
                _unitofWork.PriceRepository.Delete(priceId);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}