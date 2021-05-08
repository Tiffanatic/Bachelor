using System;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface ITimeRegistrationService
    {
        TimeSpan GetTimeRecordedForAssignment(int i);
        bool RegisterTime(TimeRecordEntity timeRecordEntity, int assignmentId);
        bool LimitTimeRecordToHoursOfTheDay(TimeRecordEntity timeRecordEntity, AssignmentEntity assignmentEntity);
    }
}