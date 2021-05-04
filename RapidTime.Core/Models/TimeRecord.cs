using System;

namespace RapidTime.Core.Models
{
    public class TimeRecord : BaseEntity
    {
        public TimeSpan TimeRecorded { get; set; }
        public DateTime Date { get; set; }
    }
}