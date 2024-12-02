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
            var d = await _db.Student.Include(x => x.ClassStudent).Include(x => x.RecordSubject).FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _db.ClassStudent.RemoveRange(d.ClassStudent);
                _db.RecordSubject.RemoveRange(d.RecordSubject);

                _unitOfWork.Student.Remove(id);
                _unitOfWork.Save();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

    }
}
