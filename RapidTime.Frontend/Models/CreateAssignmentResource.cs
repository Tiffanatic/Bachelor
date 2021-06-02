using System;

namespace RapidTime.Frontend.Models
{
    public class CreateAssignmentResource
    {
        public DateTime DateStarted { get; set; }
        public AssignmentTypeBase AssignmentType { get; set; }
    }
}