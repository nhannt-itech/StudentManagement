using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class SummarySubject
    {
        public string Id { get; set; }
        public int? SubjectId { get; set; }
        public string ClassId { get; set; }
        public int? Semester { get; set; }
        public int? PassQuantity { get; set; }
        public float? Percentage { get; set; }

        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
