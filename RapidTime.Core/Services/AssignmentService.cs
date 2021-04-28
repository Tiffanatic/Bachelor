using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private IUnitofWork _unitOfWork;
        private ILogger _logger;

        public AssignmentService(IUnitofWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        public IEnumerable<Assignment> GetAll()
        {
            return _unitOfWork.AssignmentRepository.GetAll();
        }

        public Assignment[] FindAssignmentByNameOrNumber(string nameOrNumber)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWork.AssignmentRepository.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Insert(Assignment assignment)
        {
            try
            {
                _unitOfWork.AssignmentRepository.Insert(assignment);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);;
                throw new ArgumentException(e.Message);
            }
            
        }

        public Assignment GetById(int id)
        {
            return _unitOfWork.AssignmentRepository.GetbyId(id);
        }

        public void Update(Assignment assignment)
        {
            try
            {
                _unitOfWork.AssignmentRepository.Update(assignment);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);;
                throw new ArgumentException(e.Message);
            }
        }
    }
}