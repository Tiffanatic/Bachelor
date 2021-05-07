﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Services
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
        
        public IEnumerable<AssignmentEntity> GetAll()
        {
            return _unitOfWork.AssignmentRepository.GetAll();
        }

        public AssignmentEntity[] FindAssignmentByNameOrNumber(string nameOrNumber)
        {
            // List<Assignment> assignments = GetAll().ToList();
            // var returnAssignment;
            // Assignment[] assignment;
            // if (int.TryParse(nameOrNumber, out returnAssignment))
            // {
            //     assignment = assignments.FindAll(x => x.PostalCode == postalCode.ToString()).ToArray();
            //     return assignment;
            // }
            //
            // city = cities.FindAll(x => true).Where(x => x.CityName.Contains(NameOrPostalCode)).ToArray();
            // return city;
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWork.AssignmentRepository.Delete(id);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                Console.WriteLine(e);
                throw;
            }
        }

        public int Insert(AssignmentEntity assignmentEntity)
        {
            try
            {
                var id = _unitOfWork.AssignmentRepository.Insert(assignmentEntity);
                _unitOfWork.Commit();
                return id;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                _logger.LogError("{Message}, {StackTrace}", e.Message, e.StackTrace);;
                throw new ArgumentException(e.Message);
            }
            
        }

        public AssignmentEntity GetById(int id)
        {
            return _unitOfWork.AssignmentRepository.GetbyId(id);
        }

        public void Update(AssignmentEntity assignmentEntity)
        {
            try
            {
                _unitOfWork.AssignmentRepository.Update(assignmentEntity);
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