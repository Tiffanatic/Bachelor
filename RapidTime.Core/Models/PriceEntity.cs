using System;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core.Models
{
    public class PriceEntity : BaseEntity
    {
        public double HourlyRate { get; set; }
        public int AssignmentTypeId { get; set; }
        public AssignmentTypeEntity AssignmentTypeEntity { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}