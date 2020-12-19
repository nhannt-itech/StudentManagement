using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Models.ViewModels
{
    public class SearchScoreVM
    {
        public Student Student { get; set; }
        public string AvgSem1 { get; set; } //điểm trung bình HK1
        public string AvgSem2 { get; set; }//điểm trung bình HK2
    }
}
