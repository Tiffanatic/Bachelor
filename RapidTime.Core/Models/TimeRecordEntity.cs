using System;

namespace RapidTime.Core.Models
{
    public class TimeRecordEntity : BaseEntity
    {
        public TimeSpan TimeRecorded { get; set; }
        public DateTime Date { get; set; }
        public int AssignmentId { get; set; }
        public AssignmentEntity AssignmentEntity { get; set; }
    }
}