using System;
using System.Collections.Generic;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core.Models
{
    public class Assignment : BaseEntity
    {
        public int AssignmentTypeId { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public DateTime DateStarted { get; set; }
        public double Amount { get; set; }
        public TimeSpan TimeSpentInTotal { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public List<TimeRecord> TimeRecords { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}