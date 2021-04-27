using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface ICompanyTypeService
    {
        IEnumerable<CompanyType> GetAll();
        void Delete(int companyTypeId);
        CompanyType findById(int i);
        void Update(CompanyType companyType);
        void Insert(CompanyType companyType);
    }
}