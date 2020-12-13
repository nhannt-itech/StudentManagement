using System;
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

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ScoreManageController : Controller
    {
        private readonly ILogger<ScoreManageController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public static int SubId; //Id of Subject

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
            var objList = _unitOfWork.RecordSubject.GetAll(x => x.SubjectId == id, includeProperties:"Class");
            var classList = new List<Class>();
            foreach (var obj in objList)
            {
                classList.Add(obj.Class);
            }
            return View(classList);
        }
        public IActionResult StudentList()
        {
            return View();
        }
        #region API CALL
        [HttpGet]
        public IActionResult GetAllStudent(string? id) // id of Class
        {
            var objList = _unitOfWork.RecordSubject.GetAll(x => x.ClassId == id && x.SubjectId== SubId, includeProperties: "Student");
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
