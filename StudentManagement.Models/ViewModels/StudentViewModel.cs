using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentManagement.Models.ViewModels
{
    public class StudenViewModel
    {
        public string Id { get; set; }
        [DisplayName("Họ và Tên")]
        [Required(ErrorMessage = "Phải nhập tên cho học sinh")]
        public string Name { get; set; }
        [DisplayName("Giới tính: ")]
        public string Gender { get; set; }
        [DisplayName("NGày sinh")]
        public DateTime? Birth { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        public string Email { get; set; }
        [DisplayName("Năm học")]
        public int? YearToSchool { get; set; }
        public List<string> ClassStudents { get; set; }
        public List<string> RecordSubjects { get; set; }

        public StudenViewModel()
        {
            ClassStudents = new List<string>();
            RecordSubjects = new List<string>();
        }
    }
}
