using System;
using System.Collections.Generic;
using System.Linq;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public class AssignmentTypeService : IAssignmentType
    {
        private IUnitofWork _unitofWork;
        public AssignmentTypeService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<AssignmentType> GetAll()
        {
            return _unitofWork.AssignmentTypeRepository.getAll();
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

        public AssignmentType[] GetByNameOrNumber(string input)
        {
            if (input == "") return new AssignmentType[0];
            var assignmentTypes = _unitofWork.AssignmentTypeRepository.getAll();
            
            if (int.TryParse(input, out var number))
            {
                return assignmentTypes.Where(c 
                    => Convert.ToInt32(c.Number) == number).ToArray();
            }

            return assignmentTypes.Where(c
                => c.Name.Contains(input)).ToArray();
        }

        public void Insert(AssignmentType assignmentType)
        {
            try
            {
                _unitofWork.AssignmentTypeRepository.Insert(assignmentType);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
                
            }
        }
        
        public AssignmentType GetById(int i)
        {
            return _unitofWork.AssignmentTypeRepository.GetbyId(i);
        }

        public void Update(AssignmentType assignmentType)
        {
            if (assignmentType == null) throw new NullReferenceException();
            try
            {
                _unitofWork.AssignmentTypeRepository.Update(assignmentType);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}