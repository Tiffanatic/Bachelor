using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class TimeRegistrationService : ITimeRegistrationService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IAssignmentService _assignmentService;
        
        private readonly ILogger<TimeRegistrationService> _logger;

        public TimeRegistrationService(IUnitofWork unitOfWork, IAssignmentService assignmentService, ILogger<TimeRegistrationService> logger)
        {
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
            _logger = logger;
            
        }

        public TimeSpan GetTimeRecordedForAssignment(int i)
        {
            var assignment = _unitOfWork.AssignmentRepository.GetbyId(i);
            TimeSpan timeRecordedTotal = default;
            
            foreach (var timeRecord in assignment.TimeRecords)
            {
                timeRecordedTotal = timeRecordedTotal.Add(timeRecord.TimeRecorded);
            }

            return timeRecordedTotal;
        }

        public bool RegisterTime(TimeRecordEntity timeRecordEntity, int assignmentId)
        {
            AssignmentEntity assignmentEntity = _assignmentService.GetById(assignmentId);
            
            if (assignmentEntity.DateStarted >= timeRecordEntity.Date) return false;
            
            if (timeRecordEntity.TimeRecorded.Hours > 24)
            {
                throw new Exception("Unable to register more than 24 hours a day.");
            }

            if (LimitTimeRecordToHoursOfTheDay(timeRecordEntity, assignmentEntity))
            {
                if (assignmentEntity.TimeRecords is null)
                    assignmentEntity.TimeRecords = new List<TimeRecordEntity>();
                assignmentEntity.TimeRecords.Add(timeRecordEntity);
                var entity = _unitOfWork.TimeRecordRepository.Insert(timeRecordEntity);
                _unitOfWork.Commit();
                return true;
            }

            return false;
        }

        // Helper methods
        public bool LimitTimeRecordToHoursOfTheDay(TimeRecordEntity timeRecordEntity, AssignmentEntity assignmentEntity)
        {
            var sum = 0;
            if (assignmentEntity.TimeRecords != null)
            {
                foreach (var existingTimeRecord in assignmentEntity.TimeRecords)
                {
                    sum += existingTimeRecord.TimeRecorded.Hours;
                }
            }
            if (sum > 24.0) { throw new Exception("Unable to register more than 24 hours a day.");}

            return true;
        }

        public TimeSpan GetTimeRegisteredBetweenDates(int assignmentId, DateTime startDate, DateTime endDate)
        {
            TimeSpan registeredTime = default;
            var assignment = _assignmentService.GetById(assignmentId);

            foreach (var record in assignment.TimeRecords)
            {
                if (startDate < record.Date && record.Date < endDate)
                {
                    registeredTime += record.TimeRecorded;
                }
            }

            return registeredTime;
        }

        public List<TimeRecordEntity> GetTimeRecordsForAssignment(int i)
        {
            List<TimeRecordEntity> timeRecords = new List<TimeRecordEntity>();
            var assignment = _assignmentService.GetById(i);

            foreach (TimeRecordEntity timeRecord in assignment.TimeRecords)
            {
                if (timeRecord != null)
                {
                    timeRecords.Add(timeRecord);    
                }
            }

            return timeRecords;
        }
    }
}