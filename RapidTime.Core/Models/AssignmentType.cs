namespace RapidTime.Core.Models
{
    public class AssignmentType : BaseEntity
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public bool InvoiceAble { get; set; }
    }
}