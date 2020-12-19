using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class Student
    {
        public Student()
        {
            ClassStudent = new HashSet<ClassStudent>();
            RecordSubject = new HashSet<RecordSubject>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? Birth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? YearToSchool { get; set; }

        public virtual ICollection<ClassStudent> ClassStudent { get; set; }
        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
    }
}
