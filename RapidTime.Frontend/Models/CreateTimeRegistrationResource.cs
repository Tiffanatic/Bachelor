using System;

namespace RapidTime.Frontend.Models
{
    public class CreateTimeRegistrationResource
    {
        public TimeSpan TimeRecorded { get; set; }
        public int AssignmentId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }
}