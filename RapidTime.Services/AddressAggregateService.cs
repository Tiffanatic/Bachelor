﻿using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models.Address;

namespace RapidTime.Services
{
    public class AddressAggregateService : IAddressAggregateService
    {
        private readonly IUnitofWork _unitOfWork;

        public AddressAggregateService(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AddressAggregateEntity> GetAll()
        {
            return _unitOfWork.AddressAggregateRepository.GetAll();
        }

        public void Delete(int id)
        {
            _unitOfWork.AddressAggregateRepository.Delete(id);
            _unitOfWork.Commit();
        }

        public AddressAggregateEntity FindById(int id)
        {
            return _unitOfWork.AddressAggregateRepository.GetbyId(id);
        }

        public void Update(AddressAggregateEntity addressAggregateEntity)
        {
            _unitOfWork.AddressAggregateRepository.Update(addressAggregateEntity);
            _unitOfWork.Commit();
        }

        public int Insert(AddressAggregateEntity addressAggregateEntity)
        {
            var entity = _unitOfWork.AddressAggregateRepository.Insert(addressAggregateEntity);
            _unitOfWork.Commit();
            return entity.Id;
        }
    }
}