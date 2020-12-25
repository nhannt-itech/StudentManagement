using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Models.ViewModels
{
    public class ScoreVM
    {
        public Student Student { get; set; }
        public RecordSubject RecordSubject { get; set; }
        public List<ScoreRecordSubject> ScoreList {get; set;}
    }
}
