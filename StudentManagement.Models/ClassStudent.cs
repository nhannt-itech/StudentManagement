using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public partial class ClassStudent
    {
        public string ClassId { get; set; }
        public string StudentId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
