using System;
using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class Assignment : BaseEntity
    {
        public AssignmentType AssignmentType { get; set; }
        public DateTime DateStarted { get; set; }
        public double Amount { get; set; }
        public TimeSpan TimeSpentInTotal { get; set; }
        public Customer Customer { get; set; }
        public List<TimeRecord> TimeRecords { get; set; }
    }
}