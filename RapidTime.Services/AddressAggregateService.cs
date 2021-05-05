using System;
using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class AddressAggregateService : IAddressAggregateService
    {
        private IUnitofWork _unitOfWork;

        public AddressAggregateService(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AddressAggregate> GetAll()
        {
            return _unitOfWork.AddressAggregateRepository.GetAll();
        }

        public void Delete(int id)
        {
            _unitOfWork.AddressAggregateRepository.Delete(id);
            _unitOfWork.Commit();
        }

        public AddressAggregate FindById(int id)
        {
            return _unitOfWork.AddressAggregateRepository.GetbyId(id);
        }

        public void Update(AddressAggregate addressAggregate)
        {
            _unitOfWork.AddressAggregateRepository.Update(addressAggregate);
            _unitOfWork.Commit();
        }

        public void Insert(AddressAggregate addressAggregate)
        {
            _unitOfWork.AddressAggregateRepository.Insert(addressAggregate);
            _unitOfWork.Commit();
        }
    }
}