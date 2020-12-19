﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using StudentManagement.Models.ViewModels;
using StudentManagement.Utility;
using static StudentManagement.Helper;namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ScoreManageController : Controller
    {
        private readonly ILogger<ScoreManageController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public static int SubId; //Id of Subject
        public static string ClassId;
        public static int SemesterId;

        public ScoreManageController(ILogger<ScoreManageController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            var subjectList = _unitOfWork.Subject.GetAll();
            return View(subjectList);
        }

        public IActionResult ClassList(int id) //Id of Subject
        {
            SubId = id;
            var objList = _unitOfWork.RecordSubject.GetAll(x => x.SubjectId == id, includeProperties: "Class");
            ViewBag.subject = _unitOfWork.Subject.Get(SubId).Name;
            var classList = new List<Class>();
            int check = 1;
            foreach (var obj in objList)
            {
                check = 1;
                foreach(var cls in classList)
                {
                    if(obj.Class == cls)
                    {
                        check = 0;
                    }
                }
                if(check==1)
                {
                    classList.Add(obj.Class);
                }

            }
            return View(classList);
        }
        public IActionResult SemesterList(string? id) //id of class
        {
            ViewBag.subject = _unitOfWork.Subject.Get(SubId).Name;
            ViewBag.lop = _unitOfWork.Class.Get(id).Name;
            ClassId = id;
            return View();
        }

        public IActionResult StudentScore(int id) // id of Semester
        {
            ViewBag.subject = _unitOfWork.Subject.Get(SubId).Name;
            ViewBag.lop = _unitOfWork.Class.Get(ClassId).Name;
            ViewBag.semester = "Học kỳ " + id;
            SemesterId = id;
            var recordSubjectList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == ClassId && x.SubjectId == SubId && x.Semeter == id, includeProperties: "Student");
            var scoreVMList = new List<ScoreVM>();


            foreach (var obj in recordSubjectList)
            {
                ScoreVM scoreVM = new ScoreVM();
                scoreVM.Student = obj.Student;
                scoreVM.RecordSubject = obj;
                scoreVM.ScoreList = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == obj.Id).ToList();
                scoreVMList.Add(scoreVM);
            }
            return View(scoreVMList);
        }
        [NoDirectAccess]
        public IActionResult EditScore(string id) // id of RecordSubject.Id
        {
            var scoreVM = new ScoreVM();
            var obj = _unitOfWork.RecordSubject.Get(id);
            obj.Student = _unitOfWork.Student.Get(obj.StudentId);
            obj.Subject = _unitOfWork.Subject.Get(obj.SubjectId.GetValueOrDefault());
            scoreVM.Student = obj.Student;
            scoreVM.RecordSubject = obj;
            scoreVM.ScoreList = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == obj.Id).ToList() ;

        
            if (scoreVM == null)
            {
                return NotFound();
            }
            return View(scoreVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditScore(string id, ScoreVM scoreVM) // id of RecordSubject.Id
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach(var score in scoreVM.ScoreList)
                    {
                        _unitOfWork.ScoreRecordSubject.Update(score);
                    }

                    _unitOfWork.Save();

                }
                catch 
                {
                    return NotFound();
                }

                var recordSubjectList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == id && x.SubjectId == SubId, includeProperties: "Student");
                var scoreVMList = new List<ScoreVM>();


                foreach (var obj in recordSubjectList)
                {
                    ScoreVM tempVM = new ScoreVM();
                    tempVM.Student = obj.Student;
                    tempVM.RecordSubject = obj;
                    tempVM.ScoreList = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == obj.Id).ToList();
                    scoreVMList.Add(tempVM);
                }



                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", scoreVMList) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditScore", scoreVM) });
        }

        public IActionResult AverageScore()
        {
            ViewBag.subject = _unitOfWork.Subject.Get(SubId).Name;
            ViewBag.lop = _unitOfWork.Class.Get(ClassId).Name;
            ViewBag.semester =SemesterId;
            var recordSubjectList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == ClassId && x.SubjectId == SubId && x.Semeter == SemesterId, includeProperties: "Student");
            var scoreVMList = new List<ScoreVM>();


            foreach (var obj in recordSubjectList)
            {
                ScoreVM scoreVM = new ScoreVM();
                scoreVM.Student = obj.Student;
                scoreVM.RecordSubject = obj;
                scoreVM.ScoreList = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == obj.Id).ToList();

                float? score1 = scoreVM.ScoreList.Where(x => x.RecordType == "D15P").Select(x => x.Score).FirstOrDefault();
                float? score2 = scoreVM.ScoreList.Where(x => x.RecordType == "D1T").Select(x => x.Score).FirstOrDefault();
                float? score3 = scoreVM.ScoreList.Where(x => x.RecordType == "DHK").Select(x => x.Score).FirstOrDefault();

                if (score1 == null)
                {
                    score1 = 0;

                }
                if (score2 == null)
                {
                    score2 = 0;

                }
                if (score3 == null)
                {
                    score3 = 0;

                }
                obj.Average = SD.GetAverageScore(score1, score2, score3);
                _unitOfWork.RecordSubject.Update(obj);
                _unitOfWork.Save();
                scoreVM.RecordSubject.Average = SD.GetAverageScore(score1, score2, score3);

                scoreVMList.Add(scoreVM);
            }
            return View(scoreVMList);
        }
       


        #region API CALL
        [HttpGet]
        public IActionResult GetAllStudent(string? id) // id of Class
        {
            var objList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == id && x.SubjectId == SubId, includeProperties: "Student");
            var studentList = new List<Student>();
            foreach (var obj in objList)
            {
                studentList.Add(obj.Student);
            }
            return Json(new { data = studentList });
        }
        #endregion

    }
}