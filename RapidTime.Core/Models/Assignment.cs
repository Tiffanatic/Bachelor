using System;

namespace RapidTime.Core.Models
{
    public class Assignment : BaseEntity
    {
        public int AssignmentTypeId { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string TimeSpent { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}