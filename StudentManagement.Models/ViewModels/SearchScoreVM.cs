﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Models.ViewModels
{
    public class SearchScoreVM
    {
        public Student Student { get; set; }
        public float AvgSem1 { get; set; } //điểm trung bình HK1
        public float AvgSem2 { get; set; }//điểm trung bình HK2
    }
}
