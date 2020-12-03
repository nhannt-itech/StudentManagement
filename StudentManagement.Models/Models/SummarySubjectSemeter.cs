using System;
using System.Collections.Generic;

namespace StudentManagement.Models.Models
{
    public partial class SummarySubjectSemeter
    {
        public string Id { get; set; }
        public string SubjectName { get; set; }
        public string ClassId { get; set; }
        public int? Semeter { get; set; }
        public int? PassQuantity { get; set; }
        public float? Percentage { get; set; }

        public virtual Class Class { get; set; }
    }
}
