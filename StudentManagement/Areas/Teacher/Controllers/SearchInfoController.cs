using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models.ViewModels;
using StudentManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = SD.Role_Teacher + "," + SD.Role_Manager)]
    public class SearchInfoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;


        public SearchInfoController(IUnitOfWork unitOfWork, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult SearchStudent()
        {
            return View();
        }

        public IActionResult SearchSubject()
        {
            return View();
        }

        public IActionResult SearchClass()
        {
            return View();
        }


        public IActionResult GetAllStudent()
        {
            var obj = _unitOfWork.Student.GetAll().Select(x => new
            {
                id = x.Id,
                name = x.Name,
                gender = x.Gender,
                birth = x.Birth.GetValueOrDefault().ToShortDateString(),
                address = x.Address,
                email = x.Email,
                yearToSchool = x.YearToSchool
            });
            return Json(new { data = obj });
        }

        [HttpGet]
        public IActionResult GetAllSubject()
        {
            var allObj = _unitOfWork.Subject.GetAll();
            return Json(new { data = allObj });
        }

        [HttpGet]
        public IActionResult GetAllClass()
        {
            var allObj = _unitOfWork.Class.GetAll();
            return Json(new { data = allObj });
        }


        [HttpGet]
        public IActionResult DetailsStudent(string id)
        {
            //.Include(x => x.RecordSubject).ThenInclude(x => x.Average)
            var student = _db.Student.Include(x => x.ClassStudent).ThenInclude(x => x.Class)
                .Where(x => x.Id == id).SingleOrDefault();

            string ec;
            string ec2;

            var average = _unitOfWork.RecordSubject.GetFirstOrDefault(x => x.StudentId == id);
            if (average == null)
            {
                ec = "Chưa nhập đủ điểm";
            }
            else
            {
                ec = average.Average.ToString();
            }
            if (average != null && average.Average >= 8)
            {
                ec2 = "Học sinh giỏi";
            }
            else if (average != null && average.Average >= 6.5 && average.Average < 8)
            {
                ec2 = "Học Sinh Khá";
            }
            else if (average != null && average.Average < 6.5 && average.Average > 5)
            {
                ec2 = "Học sinh Trung bình";
            }
            else if (average != null && average.Average < 5 && average.Average > 0)
                ec2 = "Học Sinh yếu";
            else
                ec2 = "Chưa xếp loại";
            var obj = new
            {
                id = student.Id,
                name = student.Name,
                gender = student.Gender,
                birth = student.Birth.GetValueOrDefault().ToShortDateString(),
                address = student.Address,
                email = student.Email,
                yearToSchool = student.YearToSchool,
                inClass = student.ClassStudent.Select(x => x.Class.Name).ToList(),
                scores = ec,
                achievements = ec2
            };
            return Json(obj);
        }

        [HttpGet]
        public IActionResult DetailsSubject(int id)
        {
            //.Include(x => x.RecordSubject).ThenInclude(x => x.Average)
            //var subject1 = _db.Subject.Include(x => x.ClassStudent).ThenInclude(x => x.Class)
            //    .Where(x => x.Id == id).SingleOrDefault();

            var subject = _unitOfWork.Subject.GetFirstOrDefault(x => x.Id == id);

            int? passQuantity1 = 0;
            int? passQuantity2 = 0;
            float? percent1 = 0;
            float? percent2 = 0;


            var summary1 = _unitOfWork.SummarySubject.GetAll(x => x.SubjectId == id && x.Semester == 1).ToList();
            foreach(var x in summary1)
            {
                percent1 += x.Percentage;
                passQuantity1 += x.PassQuantity;
                
            }        
            percent1 = (percent1 / (summary1.Count())) * 100;

            var summary2 = _unitOfWork.SummarySubject.GetAll(x => x.SubjectId == id && x.Semester == 2).ToList();
            foreach (var x in summary2)
            {
                percent2 += x.Percentage;
                passQuantity2 += x.PassQuantity;

            }
            percent2 = (percent2 / (summary2.Count())) * 100;

            string Per1 = percent1.ToString() + "%";
            string Per2 = percent2.ToString() + "%";

            var obj = new
            {
                id = subject.Id,
                name = subject.Name,
                pass1 = passQuantity1,
                pass2=passQuantity2,
                per1 = Per1,
                per2 = Per2

            };
            return Json(obj);
        }

        [HttpGet]
        public IActionResult DetailsClass(string id)
        {
            var lopHoc = _unitOfWork.Class.GetFirstOrDefault(x => x.Id == id);
            var studentList = _unitOfWork.ClassStudent.GetAll(x => x.ClassId == id, includeProperties: "Student");
            var recordList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == id);

            foreach (var u in studentList)
            {
                u.Student.RecordSubject = _unitOfWork.RecordSubject.GetAll(x => x.StudentId == u.StudentId).ToList();
            }


            ViewBag.lop = _unitOfWork.Class.Get(id).Name.ToString();


            int hsGioi = 0;
            int hsKha = 0;
            int hsTB = 0;
            int hsYeu = 0;
            var searchScoreList = new List<SearchScoreVM>();
            foreach (var st in studentList)
            {
                SearchScoreVM score = new SearchScoreVM();
                score.Student = st.Student;

                score.AvgSem1 = (float)Math.Round((float)st.Student.RecordSubject.Where(x => x.Semester == 1 && x.ClassId == id).Select(x => x.Average).Average().GetValueOrDefault(), 2);
                score.AvgSem2 = (float)Math.Round((float)st.Student.RecordSubject.Where(x => x.Semester == 2 && x.ClassId == id).Select(x => x.Average).Average().GetValueOrDefault(), 2);
                score.FinalAvg = score.FinalAvg = (float)Math.Round((score.AvgSem1 + score.AvgSem2) / 2, 2);
                searchScoreList.Add(score);
            }

            foreach(var a in searchScoreList)
            {
                if (a.FinalAvg >= 8)
                    hsGioi++;

                if (a.FinalAvg >= 6.5 && a.FinalAvg < 8)
                    hsKha++;
                if (a.FinalAvg >= 5 && a.FinalAvg < 6.5)
                    hsTB++;
                if(a.FinalAvg <5)
                {
                    hsYeu++;
                }
            }

            var obj = new
            {
                id = lopHoc.Id,
                name = lopHoc.Name,
                year = lopHoc.Year,
                numStudents = lopHoc.NumStudents,
                grade = lopHoc.Grade,
                gioi = hsGioi,
                kha = hsKha,
                tb = hsTB,
                yeu = hsYeu

            };
            return Json(obj);
        }
    }
}
