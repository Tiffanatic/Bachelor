using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class PriceService : IPriceService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<PriceService> _logger;

        public PriceService(IUnitofWork unitofWork, ILogger<PriceService> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public PriceEntity GetById(int i)
        {
            return _unitofWork.PriceRepository.GetbyId(i);
        }

        public IEnumerable<PriceEntity> GetAll()
        {
            return _unitofWork.PriceRepository.GetAll();
        }

        public int Insert(PriceEntity priceEntity)
        {
            try
            {
                var id =_unitofWork.PriceRepository.Insert(priceEntity);
                _unitofWork.Commit();
                return id.Id;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public void Update(PriceEntity priceEntity)
        {
            try
            {
                _unitofWork.PriceRepository.Update(priceEntity);
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
            if (priceId == 0) throw new ArgumentNullException();
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