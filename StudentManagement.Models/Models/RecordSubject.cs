using System;
using System.Collections.Generic;

namespace StudentManagement.Models.Models
{
    public partial class RecordSubject
    {
        public RecordSubject()
        {
            ScoredRecordSubject = new HashSet<ScoredRecordSubject>();
        }

        public string Id { get; set; }
        public string SubjectName { get; set; }
        public string ClassId { get; set; }
        public string StudentId { get; set; }
        public int? Semeter { get; set; }
        public float? Average { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<ScoredRecordSubject> ScoredRecordSubject { get; set; }
    }
}
