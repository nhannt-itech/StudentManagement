﻿using System;
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

namespace StudentManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public StudentController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SearchStudent()
        {
            return View();
        }
        public IActionResult GetAll()
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


        public async Task<IActionResult> Upsert(string? id)
        {
           if(id != null)
            {
                var student = await _db.Student.FirstOrDefaultAsync(x => x.Id == id);

                var st = new StudenViewModel()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Gender = student.Gender,
                    Birth = student.Birth,
                    Address = student.Address,
                    Email = student.Email,
                    YearToSchool = student.YearToSchool
                };
                return View(st);
            }
           else
            {
                var st = new StudenViewModel();
                return View(st);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(StudenViewModel student)
        {
            if (ModelState.IsValid)
            {
                if(student.Id == null)
                {
                    var students = new Student()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = student.Name,
                        Gender = student.Gender,
                        Birth = student.Birth,
                        Address = student.Address,
                        Email = student.Email,
                        YearToSchool = student.YearToSchool

                    };
                    _db.Student.Add(students);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var studentss = await _db.Student.FirstOrDefaultAsync(x => x.Id == student.Id);

                    studentss.Id = student.Id;
                    studentss.Name = student.Name;
                    studentss.Gender = student.Gender;
                    studentss.Birth = student.Birth;
                    studentss.Address = student.Address;
                    studentss.Email = student.Email;
                    studentss.YearToSchool = student.YearToSchool;

                    _db.Student.Update(studentss);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction();
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            //lấy ra học sinh cần xóa
            var student = await _db.Student.Include(x => x.ClassStudent).Include(x => x.RecordSubject).FirstOrDefaultAsync(x => x.Id == id);


            //Lấy ra lớp học nào mà có học sinh bị xóa để trừ đi sỉ số
            try
            {
                var classStudent = _unitOfWork.ClassStudent.GetFirstOrDefault(x => x.StudentId == id);
                var lophoc = _db.Class.FirstOrDefault(x => x.Id == classStudent.ClassId);
                lophoc.NumStudents--;
            }
            catch // Lỡ mà xóa trung học sinh chưa được thêm vào lớp nên bỏ vào trycatch
            {
            }
            //Lấy ra mấy cái điểm của học sinh đó để xóa đi
            List<RecordSubject> recordSubject = _unitOfWork.RecordSubject.GetAll(x => x.StudentId == id).ToList();
            foreach (var c in recordSubject)
            {
                List<ScoreRecordSubject> obj = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == c.Id).ToList();
                foreach (var a in obj)
                {
                    _unitOfWork.ScoreRecordSubject.Remove(a);
                }
                _unitOfWork.Save();
            }
            try
            {

                _db.ClassStudent.RemoveRange(student.ClassStudent);

                _db.RecordSubject.RemoveRange(student.RecordSubject);

                _db.Student.Remove(student);

                
                _db.SaveChanges();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            //.Include(x => x.RecordSubject).ThenInclude(x => x.Average)
            var student = _db.Student.Include(x => x.ClassStudent).ThenInclude(x => x.Class)               
                .Where(x => x.Id == id).SingleOrDefault();

            string ec;
            string ec2;

            var average = _unitOfWork.RecordSubject.GetFirstOrDefault(x => x.StudentId == id);
            if(average == null)
            {
                ec  = "Chưa nhập đủ điểm";
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
    }
}
