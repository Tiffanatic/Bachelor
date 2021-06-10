using System;

namespace RapidTime.Frontend.Models
{
    public class CreateAssignmentResource
    {
        public DateTime DateStarted { get; set; }
        public int AssignmentTypeId { get; set; }
    }
}