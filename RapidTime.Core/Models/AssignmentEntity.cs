using System;
using System.Collections.Generic;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core.Models
{
    public class AssignmentEntity : BaseEntity
    {
        public int AssignmentTypeId { get; set; }
        public AssignmentTypeEntity AssignmentTypeEntity { get; set; }
        public DateTime DateStarted { get; set; }
        public double Amount { get; set; }
        public TimeSpan TimeSpentInTotal { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        public int CustomerId { get; set; }
        public List<TimeRecordEntity> TimeRecords { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}