using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassStudent = new HashSet<ClassStudent>();
            RecordSubject = new HashSet<RecordSubject>();
            Summary = new HashSet<Summary>();
            SummarySubject = new HashSet<SummarySubject>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int? NumStudents { get; set; }
        public int? Grade { get; set; }

        public virtual ICollection<ClassStudent> ClassStudent { get; set; }
        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
        public virtual ICollection<Summary> Summary { get; set; }
        public virtual ICollection<SummarySubject> SummarySubject { get; set; }
    }
}
