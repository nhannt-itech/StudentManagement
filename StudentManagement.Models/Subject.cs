using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Subject
    {
        public Subject()
        {
            RecordSubject = new HashSet<RecordSubject>();
            SummarySubject = new HashSet<SummarySubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
        public virtual ICollection<SummarySubject> SummarySubject { get; set; }
    }
}
