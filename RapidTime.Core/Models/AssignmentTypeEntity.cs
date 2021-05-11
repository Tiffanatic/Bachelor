using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class AssignmentTypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public bool InvoiceAble { get; set; }
        public IList<AssignmentEntity> Assignments { get; set; }
        public IList<PriceEntity> Prices { get; set; }
    }
}