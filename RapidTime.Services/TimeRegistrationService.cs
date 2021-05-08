using System;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class TimeRegistrationService : ITimeRegistrationService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly AssignmentService _assignmentService;
        private readonly ILogger<TimeRegistrationService> _logger;

        public TimeRegistrationService(IUnitofWork unitOfWork, AssignmentService assignmentService, ILogger<TimeRegistrationService> logger)
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
            
            if (assignmentEntity.DateStarted <= timeRecordEntity.Date) return false;
            
            if (timeRecordEntity.TimeRecorded.Hours > 24)
            {
                throw new Exception("Unable to register more than 24 hours a day.");
            }

            if (LimitTimeRecordToHoursOfTheDay(timeRecordEntity, assignmentEntity))
            {
                assignmentEntity.TimeRecords.Add(timeRecordEntity);
                _unitOfWork.Commit();
            }

            return false;
        }

        // Helper methods
        public bool LimitTimeRecordToHoursOfTheDay(TimeRecordEntity timeRecordEntity, AssignmentEntity assignmentEntity)
        {
            var sum = 0;

            foreach (var existingTimeRecord in assignmentEntity.TimeRecords)
            {
                sum += existingTimeRecord.TimeRecorded.Hours;
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
    }
}