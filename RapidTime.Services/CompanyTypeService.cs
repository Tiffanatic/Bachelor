using System;
using System.Collections;
using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private IUnitofWork _unitofWork;

        public CompanyTypeService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<CompanyType> GetAll()
        {
            return _unitofWork.CompanyTypeRepository.GetAll();
        }

        public void Delete(int companyTypeId)
        {
            if (companyTypeId == null) throw new NullReferenceException();
            try
            {
                _unitofWork.CompanyTypeRepository.Delete(companyTypeId);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CompanyType findById(int i)
        {
            return _unitofWork.CompanyTypeRepository.GetbyId(i);
        }

        public void Update(CompanyType companyType)
        {
            try
            {
                _unitofWork.CompanyTypeRepository.Update(companyType);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void Insert(CompanyType companyType)
        {
            try
            {
                _unitofWork.CompanyTypeRepository.Insert(companyType);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
    }
    
}