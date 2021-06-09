namespace RapidTime.Frontend.Models
{
    public class CreatePriceResource
    {
        public AssignmentTypeResponse AssignmentType { get; set; }
        public string Id { get; set; }
        public double HourlyRate { get; set; }
        
    }
}