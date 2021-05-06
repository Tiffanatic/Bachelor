using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<CountryService> _logger;

        public CountryService(IUnitofWork unitofWork, ILogger<CountryService> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public IEnumerable<CountryEntity> GetAllCountries()
        {
            
        return _unitofWork.CountryRepository.GetAll();
        
        }

        public void DeleteCountry(int countryId)
        {
            try
            {
                _unitofWork.CountryRepository.Delete(countryId);
                _unitofWork.Commit();
            }
            catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }

            
        }

        public CountryEntity[] GetCountryByNameOrCountryCode(string input)
        {
            try
            {
                var countries = GetAllCountries().ToList();
                if (input.Length < 3)
                {
                    return countries.Where(x => x.CountryCode.Contains(input)).ToArray();
                }

                return countries.Where(x => x.CountryName.Contains(input)).ToArray();
            }
            catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
            return null;
        }

        public CountryEntity FindById(int id)
        {
            try
            {
                var countries = GetAllCountries().ToList();
                return countries.SingleOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
            return null;
        }

        public void Insert(CountryEntity countryEntity)
        {
            try
            {
                _unitofWork.CountryRepository.Insert(countryEntity);
                _unitofWork.Commit();
            } 
            catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
        }

        public void Update(CountryEntity countryEntity)
        {
            try
            {
                _unitofWork.CountryRepository.Update(countryEntity);
                _unitofWork.Commit();
            } catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
        }

    }
}