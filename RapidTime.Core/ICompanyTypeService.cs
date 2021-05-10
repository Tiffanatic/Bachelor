using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface ICompanyTypeService
    {
        IEnumerable<CompanyTypeEntity> GetAll();
        void Delete(int companyTypeId);
        CompanyTypeEntity findById(int i);
        void Update(CompanyTypeEntity companyTypeEntity);
        int Insert(CompanyTypeEntity companyTypeEntity);
    }
}