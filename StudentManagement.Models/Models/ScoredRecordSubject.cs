using System;
using System.Collections.Generic;

namespace StudentManagement.Models.Models
{
    public partial class ScoredRecordSubject
    {
        public string Id { get; set; }
        public string RecordSubjectId { get; set; }
        public string RecordType { get; set; }
        public float? Scored { get; set; }

        public virtual RecordSubject RecordSubject { get; set; }
    }
}
