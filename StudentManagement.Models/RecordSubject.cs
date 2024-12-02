using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class RecordSubject
    {
        public RecordSubject()
        {
            ScoreRecordSubject = new HashSet<ScoreRecordSubject>();
        }

        public string Id { get; set; }
        public int? SubjectId { get; set; }
        public string ClassId { get; set; }
        public string StudentId { get; set; }
        public int? Semester { get; set; }
        public float? Average { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ScoreRecordSubject> ScoreRecordSubject { get; set; }
    }
}
