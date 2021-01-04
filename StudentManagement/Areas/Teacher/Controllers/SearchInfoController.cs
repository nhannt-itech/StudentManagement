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

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
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

            int? passQuantity = 0;
            float? percent = 0;

            var summary = _unitOfWork.SummarySubject.GetFirstOrDefault(x => x.SubjectId == id);
            if (summary != null)
            {
                passQuantity = summary.PassQuantity;
                percent = summary.Percentage;
            }


            var obj = new
            {
                id = subject.Id,
                name = subject.Name,
                pass = passQuantity,
                per = percent
            };
            return Json(obj);
        }

        [HttpGet]
        public IActionResult DetailsClass(string id)
        {
            var lopHoc = _unitOfWork.Class.GetFirstOrDefault(x => x.Id == id);


            // var summary = (_unitOfWork.RecordSubject.GetFirstOrDefault(x => x.ClassId == id && x.Average > 8));
            var hocSinhGioi = (_db.RecordSubject.Where(x => x.Average >= 8 && x.ClassId == id)).Count();
            var hocSinhKha = (_db.RecordSubject.Where(x => x.Average >= 6.5 && x.Average < 8 && x.Class.Id == id)).Count();
            var hocSinhTB = (_db.RecordSubject.Where(x => x.Average >= 5 && x.Average < 6.5 && x.Class.Id == id)).Count();
            var hocSinhYeu = (_db.RecordSubject.Where(x => x.Average <= 5 && x.Class.Id == id)).Count();

            var obj = new
            {
                id = lopHoc.Id,
                name = lopHoc.Name,
                year = lopHoc.Year,
                numStudents = lopHoc.NumStudents,
                grade = lopHoc.Grade,
                gioi = hocSinhGioi,
                kha = hocSinhKha,
                tb = hocSinhTB,
                yeu = hocSinhYeu

            };
            return Json(obj);
        }
    }
}
