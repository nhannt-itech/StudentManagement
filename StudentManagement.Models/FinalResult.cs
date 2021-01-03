using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentManagement.Models
{
    public class FinalResult
    {
        [Key]
        public string StudentId { get; set; }
        public string ClassId { get; set; }
        public float AvgSem1 { get; set;  }
        public float AvgSem2 { get; set; }
        public float Final { get; set;  }
        public string Rate { get; set; }
    }
}
