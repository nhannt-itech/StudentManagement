using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ClassController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public ClassController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchClass()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(string? id)
        {
            Class @class = new Class();
            if (id == null)
            {
                @class.NumStudents = 0;
                @class.Year = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            }
            else
            {
                @class = _unitOfWork.Class.GetFirstOrDefault(x => x.Id == id);
            }
            return View(@class);
        }

        [HttpPost]
        public IActionResult Upsert(Class @class)
        {
            if (ModelState.IsValid)
            {
                if (@class.Id == null)
                {
                    @class.Id = Guid.NewGuid().ToString();
                    _unitOfWork.Class.Add(@class);
                    _unitOfWork.Save();

                    CreateSummary(@class.Id); // Tạo ra bảng tổng kết
                    return View("Index");
                }
                else
                {
                    _unitOfWork.Class.Update(@class);
                    _unitOfWork.Save();
                    return View(@class);
                }
            }
            return View(@class);
        }

        #region API_Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Class.GetAll();
            return Json(new { data = allObj });
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            var allObj = _unitOfWork.Student.GetAll();
            IEnumerable<SelectListItem> StudentList = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name + " - Mã Học Sinh: " + i.Id,
                Value = i.Id
            });
            return Json(StudentList);
        }

        [HttpGet]
        public IActionResult GetStudentNotInClass()
        {
            var allObjStudentInClass = _unitOfWork.ClassStudent.GetAll(x => x.Class.Year == DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString()
                                        , includeProperties: "Class,Student").Select(x => x.Student);
            var allObjStudentNotInClass = _unitOfWork.Student.GetAll().Where(x => !allObjStudentInClass.Any(y => y.Id == x.Id));

            IEnumerable<SelectListItem> StudentList = allObjStudentNotInClass.Select(i => new SelectListItem
            {
                Text = i.Name + " - Mã Học Sinh: " + i.Id,
                Value = i.Id
            });
            return Json(StudentList);
        }

        

        [HttpGet]
        public IActionResult GetStudentInClass(string? id)
        {
            var allObjStudentInClass = _unitOfWork.ClassStudent.GetAll(x => x.Class.Id == id
                                        , includeProperties: "Class,Student").Select(x => x.Student);
            return Json(new { data = allObjStudentInClass });
        }

        public IActionResult AddStudentInClass(string classId, string studentId)
        {
            if (studentId == "null")
            {
                return Json(new { success = false, message = "Thêm học sinh lỗi!" });
            }
            else if (_unitOfWork.ClassStudent.GetAll(x => x.ClassId == classId).Count() >= 40)
            {
                return Json(new { success = false, message = "Lớp đã đạt 40 học sinh!" });
            }
            ClassStudent classStudent = new ClassStudent()
            {
                ClassId = classId,
                StudentId = studentId
            };
            //--------------------Tăng sỉ số lớp--------------------
            var classObj = _unitOfWork.Class.Get(classId);
            classObj.NumStudents++;
            _unitOfWork.Class.Update(classObj);
            //--------------------Thêm học sinh vào lớp--------------------
            _unitOfWork.ClassStudent.Add(classStudent);
            _unitOfWork.Save();
            CreateRecordStudent(classId, studentId);
            return Json(new { success = true, message = "Bạn đã thêm học sinh thành công!" });
        }       

        [HttpGet]
        public IActionResult SelectGradeSetClassName(int grade, string year)
        {
            var allObj = _unitOfWork.Class.GetAll(x => x.Grade == grade && x.Year == year);
            int max, count = allObj.Count();
            string name;
            try
            {
                max = allObj.Max(x => Convert.ToInt32(x.Name.Replace(grade.ToString() + "/", "")));
            }
            catch
            {
                max = 0;
            }
            if (count != 0 && count != max)
            {
                for (int i = 1; i < max; i++)
                {
                    if (allObj.Count(x => x.Name.Replace(grade.ToString() + "/", "") == i.ToString()) == 0)
                    {
                        name = grade.ToString() + "/" + i.ToString();
                        return Json(new { name = name });
                    }
                }
            }
            name = grade.ToString() + "/" + (count + 1).ToString();
            return Json(new { name = name });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.Class.Get(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }

            foreach (var item in _unitOfWork.RecordSubject.GetAll(x => x.ClassId == id))
            {
                foreach(var srs in _unitOfWork.ScoreRecordSubject.GetAll(x=>x.RecordSubjectId == item.Id))
                {
                    _unitOfWork.ScoreRecordSubject.Remove(srs);
                    _unitOfWork.Save();
                }
                _unitOfWork.RecordSubject.Remove(item);
                _unitOfWork.Save();
            }

            foreach (var item in _unitOfWork.ClassStudent.GetAll(x=>x.ClassId == id))
            {
                _unitOfWork.ClassStudent.Remove(item);
                _unitOfWork.Save();
            }

            foreach (var item in _unitOfWork.Summary.GetAll(x=> x.ClassId == id))
            {
                _unitOfWork.Summary.Remove(item);
                _unitOfWork.Save();
            }
            foreach (var item in _unitOfWork.SummarySubject.GetAll(x => x.ClassId == id))
            {
                _unitOfWork.SummarySubject.Remove(item);
                _unitOfWork.Save();
            }

            _unitOfWork.Class.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }

        [HttpDelete]
        public IActionResult DeleteStudentFromClass(string? classId, string studentId)
        {
            foreach(var rc in _unitOfWork.RecordSubject.GetAll(x => x.StudentId == studentId && x.ClassId == classId))
            {
                foreach(var src in _unitOfWork.ScoreRecordSubject.GetAll(x=>x.RecordSubjectId == rc.Id))
                {
                    _unitOfWork.ScoreRecordSubject.Remove(src);
                    _unitOfWork.Save();
                }
                _unitOfWork.RecordSubject.Remove(rc);
                _unitOfWork.Save();
            }

            var obj = _unitOfWork.ClassStudent.GetFirstOrDefault(x => x.StudentId == studentId && x.ClassId == classId);
            _unitOfWork.ClassStudent.Remove(obj);
            _unitOfWork.Save();

            var classObj = _unitOfWork.Class.Get(classId);
            classObj.NumStudents--;
            _unitOfWork.Class.Update(classObj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

        #region ValidationOnModal
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsGradeValid(int Grade, string Year)
        {
            if (Grade == 10)
            {
                if (_unitOfWork.Class.GetAll(x => x.Grade == Grade && x.Year == Year).Count() == 4) //THAY ĐỔI QUY ĐỊNH
                {
                    return Json(false);
                }
            }
            if (Grade == 11)
            {
                if (_unitOfWork.Class.GetAll(x => x.Grade == Grade && x.Year == Year).Count() == 3) //THAY ĐỔI QUY ĐỊNH
                {
                    return Json(false);
                }
            }
            if (Grade == 12)
            {
                if (_unitOfWork.Class.GetAll(x => x.Grade == Grade && x.Year == Year).Count() == 2) //THAY ĐỔI QUY ĐỊNH
                {
                    return Json(false);
                }
            }
            return Json(true);
        }
        #endregion

        #region ExtensionFunc
        public void CreateRecordStudent(string classId, string studentId)
        {
            string[] typeRecord = new string[3] { "D15P", "D1T", "DHK" };
            foreach (var item in _unitOfWork.Subject.GetAll())
            {
                RecordSubject recordSubject1 = new RecordSubject()
                {
                    Id = System.Guid.NewGuid().ToString(),
                    ClassId = classId,
                    StudentId = studentId,
                    Semester = 1,
                    SubjectId = item.Id,
                    Average = null
                };
                RecordSubject recordSubject2 = new RecordSubject()
                {
                    Id = System.Guid.NewGuid().ToString(),
                    ClassId = classId,
                    StudentId = studentId,
                    Semester = 2,
                    SubjectId = item.Id,
                    Average = null
                };
                _unitOfWork.RecordSubject.Add(recordSubject1);
                _unitOfWork.RecordSubject.Add(recordSubject2);
                _unitOfWork.Save();

                foreach (var tr in typeRecord)
                {
                    ScoreRecordSubject scoreRecordSubject1 = new ScoreRecordSubject()
                    {
                        Id = System.Guid.NewGuid().ToString(),
                        RecordSubjectId = recordSubject1.Id,
                        RecordType = tr,
                        Score = null
                    };
                    ScoreRecordSubject scoreRecordSubject2 = new ScoreRecordSubject()
                    {
                        Id = System.Guid.NewGuid().ToString(),
                        RecordSubjectId = recordSubject1.Id,
                        RecordType = tr,
                        Score = null
                    };
                    _unitOfWork.ScoreRecordSubject.Add(scoreRecordSubject1);
                    _unitOfWork.ScoreRecordSubject.Add(scoreRecordSubject2);
                    _unitOfWork.Save();
                }
            }
        }
        public void CreateSummary(string classId)
        {
            foreach (var item in _unitOfWork.Subject.GetAll())
            {
                SummarySubject summarySubject1 = new SummarySubject()
                {
                    Id = System.Guid.NewGuid().ToString(),
                    SubjectId = item.Id,
                    ClassId = classId,
                    Semester = 1,
                    PassQuantity = 0,
                    Percentage = 0
                };
                SummarySubject summarySubject2 = new SummarySubject()
                {
                    Id = System.Guid.NewGuid().ToString(),
                    SubjectId = item.Id,
                    ClassId = classId,
                    Semester = 2,
                    PassQuantity = 0,
                    Percentage = 0
                };
                _unitOfWork.SummarySubject.Add(summarySubject1);
                _unitOfWork.SummarySubject.Add(summarySubject2);
                _unitOfWork.Save();
            }

            Summary summary1 = new Summary()
            {
                Id = System.Guid.NewGuid().ToString(),
                ClassId = classId,
                Semester = 1,
                PassQuantity = 0,
                Percentage = 0
            };
            Summary summary2 = new Summary()
            {
                Id = System.Guid.NewGuid().ToString(),
                ClassId = classId,
                Semester = 2,
                PassQuantity = 0,
                Percentage = 0
            };
            _unitOfWork.Summary.Add(summary1);
            _unitOfWork.Summary.Add(summary2);
            _unitOfWork.Save();
        }
        #endregion

        [HttpGet]
        public IActionResult Details(string id)
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
                gioi=hocSinhGioi,
                kha=hocSinhKha,
                tb=hocSinhTB,
                yeu=hocSinhYeu

            };
            return Json(obj);
        }
    }
}
