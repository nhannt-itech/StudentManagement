using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public partial class Student
    {
        public Student()
        {
            ClassStudent = new HashSet<ClassStudent>();
            RecordSubject = new HashSet<RecordSubject>();
        }
        [NotMapped]
        public string IdAdd { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên học sinh.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập giới tính.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập ngày sinh.")]
        public DateTime? Birth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? YearToSchool { get; set; }

        public virtual ICollection<ClassStudent> ClassStudent { get; set; }
        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
    }
}
