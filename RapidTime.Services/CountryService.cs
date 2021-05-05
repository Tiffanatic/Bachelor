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
        private ILogger _logger;

        public CountryService(IUnitofWork unitofWork, ILogger logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public IEnumerable<Country> GetAllCountries()
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

        public Country[] GetCountryByNameOrCountryCode(string input)
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

        public Country FindById(int id)
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

        public void Insert(Country country)
        {
            try
            {
                _unitofWork.CountryRepository.Insert(country);
                _unitofWork.Commit();
            } 
            catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
        }

        public void Update(Country country)
        {
            try
            {
                _unitofWork.CountryRepository.Update(country);
                _unitofWork.Commit();
            } catch (Exception ex)
            {
                _unitofWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", ex.Message, ex.StackTrace);
            }
        }

    }
}