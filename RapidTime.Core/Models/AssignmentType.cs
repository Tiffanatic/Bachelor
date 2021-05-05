using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class AssignmentType : BaseEntity
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public bool InvoiceAble { get; set; }
        public IList<Assignment> Assignments { get; set; }
        public IList<Price> Prices { get; set; }
    }
}