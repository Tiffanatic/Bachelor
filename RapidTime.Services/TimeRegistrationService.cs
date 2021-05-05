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
        private readonly ILogger _logger;

        public TimeRegistrationService(IUnitofWork unitOfWork, AssignmentService assignmentService, ILogger logger)
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

        public bool RegisterTime(TimeRecord timeRecord, int assignmentId)
        {
            Assignment assignment = _assignmentService.GetById(assignmentId);
            
            if (assignment.DateStarted <= timeRecord.Date) return false;
            
            if (timeRecord.TimeRecorded.Hours > 24)
            {
                throw new Exception("Unable to register more than 24 hours a day.");
            }

            if (LimitTimeRecordToHoursOfTheDay(timeRecord, assignment))
            {
                assignment.TimeRecords.Add(timeRecord);    
            }

            return false;
        }

        // Helper methods
        private bool LimitTimeRecordToHoursOfTheDay(TimeRecord timeRecord, Assignment assignment)
        {
            var sum = 0;

            foreach (var existingTimeRecord in assignment.TimeRecords)
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