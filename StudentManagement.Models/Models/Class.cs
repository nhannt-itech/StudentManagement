using System;
using System.Collections.Generic;

namespace StudentManagement.Models.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassStudent = new HashSet<ClassStudent>();
            RecordSubject = new HashSet<RecordSubject>();
            SummarySubject = new HashSet<SummarySubject>();
            SummarySubjectSemeter = new HashSet<SummarySubjectSemeter>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int? NumStudents { get; set; }
        public int? Grade { get; set; }

        public virtual ICollection<ClassStudent> ClassStudent { get; set; }
        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
        public virtual ICollection<SummarySubject> SummarySubject { get; set; }
        public virtual ICollection<SummarySubjectSemeter> SummarySubjectSemeter { get; set; }
    }
}
