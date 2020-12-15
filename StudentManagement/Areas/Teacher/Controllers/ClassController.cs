using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ClassController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClassController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
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

            //return Json(new { data = allObjStudentNotInClass });
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
            ClassStudent classStudent = new ClassStudent()
            {
                ClassId = classId,
                StudentId = studentId
            };
            _unitOfWork.ClassStudent.Add(classStudent);
            _unitOfWork.Save();
            CreateRecordStudent(classId, studentId);
            return Json(new { success = true, message = "Bạn đã thêm học sinh thành công!" });
        }

        public void CreateRecordStudent(string classId, string studentId)
        {
            string[] typeRecord = new string[3] { "15minutes", "45minutes", "Final" };
            foreach (var item in _unitOfWork.Subject.GetAll())
            {
                RecordSubject recordSubject1 = new RecordSubject()
                {
                    Id = new Guid().ToString(),
                    ClassId = classId,
                    StudentId = studentId,
                    Semeter = 1,
                    SubjectId = item.Id,
                    Average = 0
                };
                RecordSubject recordSubject2 = new RecordSubject()
                {
                    Id = new Guid().ToString(),
                    ClassId = classId,
                    StudentId = studentId,
                    Semeter = 1,
                    SubjectId = item.Id,
                    Average = 0
                };
                _unitOfWork.RecordSubject.Add(recordSubject1);
                _unitOfWork.RecordSubject.Add(recordSubject2);
                _unitOfWork.Save();

                foreach (var tr in typeRecord)
                {
                    ScoreRecordSubject scoreRecordSubject1 = new ScoreRecordSubject()
                    {
                        Id = new Guid().ToString(),
                        RecordSubjectId = recordSubject1.Id,
                        RecordType = tr,
                        Score = 0
                    };
                    ScoreRecordSubject scoreRecordSubject2 = new ScoreRecordSubject()
                    {
                        Id = new Guid().ToString(),
                        RecordSubjectId = recordSubject1.Id,
                        RecordType = tr,
                        Score = 0
                    };
                    _unitOfWork.ScoreRecordSubject.Add(scoreRecordSubject1);
                    _unitOfWork.ScoreRecordSubject.Add(scoreRecordSubject2);
                    _unitOfWork.Save();
                }
            }
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
    }
}
