using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Bạn phải nhập tên lớp học.")]
        public string Name { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public int? NumStudents { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập khối.")]
        [Remote("IsGradeValid", "Class", "Teacher", ErrorMessage = "Lớp trong khối này đã đủ!", AdditionalFields = nameof(Year))]
        public int? Grade { get; set; }

        public virtual ICollection<ClassStudent> ClassStudent { get; set; }
        public virtual ICollection<RecordSubject> RecordSubject { get; set; }
        public virtual ICollection<Summary> Summary { get; set; }
        public virtual ICollection<SummarySubject> SummarySubject { get; set; }
    }
}
