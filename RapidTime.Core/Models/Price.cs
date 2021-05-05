using System;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core.Models
{
    public class Price : BaseEntity
    {
        public double HourlyRate { get; set; }
        public int AssignmentId { get; set; }
        public AssignmentType AssignmentType { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}