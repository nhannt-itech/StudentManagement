using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class ScoreRecordSubject
    {
        public string Id { get; set; }
        public string RecordSubjectId { get; set; }
        public string RecordType { get; set; }
        public float? Score { get; set; }

        public virtual RecordSubject RecordSubject { get; set; }
    }
}
