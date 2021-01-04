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

namespace StudentManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;


        public StudentController(IUnitOfWork unitOfWork, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
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
            if (id != null)
            {
                var student = await _db.Student.FirstOrDefaultAsync(x => x.Id == id);
                var st = new Student()
                {
                    IdAdd = student.Id,
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
                var allObj = _unitOfWork.Student.GetAll(x => x.YearToSchool == DateTime.Now.Year);
                string newStudentId;
                int max, count = allObj.Count();
                try
                {
                    max = allObj.Max(x => Convert.ToInt32(x.Id.Replace("Stud" + DateTime.Now.Year.ToString(), "")));
                }
                catch
                {
                    max = 0;
                }
                if (count != 0 && count != max)
                {
                    for (int i = 1; i < max; i++)
                    {
                        if (allObj.Count(x => x.Name.Replace("Stud" + DateTime.Now.Year.ToString() + "/", "") == i.ToString()) == 0)
                        {
                            newStudentId = "Stud" + DateTime.Now.Year.ToString() + i.ToString();
                            var student1 = new Student()
                            {
                                Id = newStudentId,
                                YearToSchool = DateTime.Now.Year,
                                Email = newStudentId + "@gmail.com"
                            };
                            return View(student1);
                        }
                    }
                }
                newStudentId = "Stud" + DateTime.Now.Year.ToString() + (count + 1).ToString();
                var student2 = new Student()
                {
                    Id = newStudentId,
                    YearToSchool = DateTime.Now.Year,
                    Email = newStudentId + "@gmail.com"
                };
                return View(student2);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.IdAdd == null)
                {
                    var students = new Student()
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Gender = student.Gender,
                        Birth = student.Birth,
                        Address = student.Address,
                        Email = student.Email,
                        YearToSchool = student.YearToSchool
                    };
                    _db.Student.Add(students);
                    await _db.SaveChangesAsync();

                    //var user = new ApplicationUser()
                    //{
                    //    UserName = student.Email,
                    //    Email = student.Email,
                    //    Name = student.Name,
                    //    PhoneNumber = "",
                    //    Address = student.Address,
                    //    Role = SD.Role_Student
                    //};

                    //await _userManager.CreateAsync(user, "Student123@"); //After is password
 
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

        [AcceptVerbs("Get", "Post")]
        public JsonResult checkAge(DateTime Birth)
        {
            int age = DateTime.Now.Year - Birth.Year;
            int ageMax = _unitOfWork.Rule.GetFirstOrDefault(x => x.Name == "Tuổi học sinh").Max;
            int ageMin = _unitOfWork.Rule.GetFirstOrDefault(x => x.Name == "Tuổi học sinh").Min;

            if (age>=ageMin && age <=ageMax)
            {
                return Json(true);
            }

            return Json(false);
        }

    }
}
