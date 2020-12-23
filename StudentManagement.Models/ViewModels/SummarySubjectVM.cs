using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Models.ViewModels
{
    public class SummarySubjectVM
    {
        public string SubjectId { get; set; }
        public string Year { get; set; }
        public IEnumerable<SelectListItem> SubjectList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
    }
}
