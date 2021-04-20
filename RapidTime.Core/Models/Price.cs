namespace RapidTime.Core.Models
{
    public class Price : BaseEntity
    {
        public double HourlyRate { get; set; }
        public AssignmentType AssignmentType { get; set; }
    }
}