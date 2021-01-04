using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public partial class Rule   
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập giá trị tối thiểu.")]
        public int Min { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập giá trị tối đa.")]
        public int Max { get; set; }

    }
}
