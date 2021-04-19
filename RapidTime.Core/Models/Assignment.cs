using System;

namespace RapidTime.Core.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string TimeSpent { get; set; }
        public Customer Customer { get; set; }

    }
}