using System;
using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private IUnitofWork _unitofWork;

        public CompanyTypeService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<CompanyTypeEntity> GetAll()
        {
            return _unitofWork.CompanyTypeRepository.GetAll();
        }

        public void Delete(int companyTypeId)
        {
            if (companyTypeId == 0) throw new ArgumentException();
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

        public CompanyTypeEntity FindById(int i)
        {
            return _unitofWork.CompanyTypeRepository.GetbyId(i);
        }

        public void Update(CompanyTypeEntity companyTypeEntity)
        {
            try
            {
                _unitofWork.CompanyTypeRepository.Update(companyTypeEntity);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int Insert(CompanyTypeEntity companyTypeEntity)
        {
            try
            {
                var id =_unitofWork.CompanyTypeRepository.Insert(companyTypeEntity);
                _unitofWork.Commit();
                return id.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
    }
    
}