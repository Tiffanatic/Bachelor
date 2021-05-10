using System;
using System.Collections.Generic;
using System.Linq;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class AssignmentTypeService : IAssignmentTypeService
    {
        private IUnitofWork _unitofWork;
        public AssignmentTypeService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<AssignmentTypeEntity> GetAll()
        {
            return _unitofWork.AssignmentTypeRepository.GetAll();
        }

        public void Delete(int assignmentTypeId)
        {
            try
            {
                _unitofWork.AssignmentTypeRepository.Delete(assignmentTypeId);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public AssignmentTypeEntity[] GetByNameOrNumber(string input)
        {
            if (input == "") return new AssignmentTypeEntity[0];
            var assignmentTypes = _unitofWork.AssignmentTypeRepository.GetAll();
            
            if (int.TryParse(input, out var number))
            {
                return assignmentTypes.Where(c 
                    => Convert.ToInt32(c.Number) == number).ToArray();
            }

            return assignmentTypes.Where(c
                => c.Name.Contains(input)).ToArray();
        }

        public int Insert(AssignmentTypeEntity assignmentTypeEntity)
        {
            try
            {
                var assignmentType =_unitofWork.AssignmentTypeRepository.Insert(assignmentTypeEntity);
                _unitofWork.Commit();
                return assignmentType.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
                
            }
        }
        
        public AssignmentTypeEntity GetById(int i)
        {
            return _unitofWork.AssignmentTypeRepository.GetbyId(i);
        }

        public void Update(AssignmentTypeEntity assignmentTypeEntity)
        {
            if (assignmentTypeEntity == null) throw new NullReferenceException();
            try
            {
                _unitofWork.AssignmentTypeRepository.Update(assignmentTypeEntity);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}