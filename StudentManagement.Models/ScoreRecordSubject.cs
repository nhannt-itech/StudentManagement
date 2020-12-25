using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public partial class ScoreRecordSubject
    {
        public string Id { get; set; }
        public string RecordSubjectId { get; set; }
        public string RecordType { get; set; }
        [Range(0,10)]
        public float? Score { get; set; }

        public virtual RecordSubject RecordSubject { get; set; }
    }
}
