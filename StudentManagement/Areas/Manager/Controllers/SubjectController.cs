using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Manager")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public SubjectController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Upsert(int? id)
        {
            Subject subject = new Subject();
            if (id == null)
            {
                //create new category
                return View(subject);
            }
            //this is for edit
            subject = _unitOfWork.Subject.Get(id.GetValueOrDefault());
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (subject.Id == 0)
                {
                    int count = _unitOfWork.Subject.GetAll().Count();
                    count = count + 1;
                    subject.Id = count;
                    _unitOfWork.Subject.Add(subject);
                }
                else
                {
                    _unitOfWork.Subject.Update(subject);
                }

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var ObjfromDb = _unitOfWork.Subject.Get(id);
            var recordList = _unitOfWork.RecordSubject.GetAll(x => x.SubjectId == id);
            foreach (var rs in recordList)
            {
                var scoreList = _unitOfWork.ScoreRecordSubject.GetAll(x => x.RecordSubjectId == rs.Id);
                foreach (var s in scoreList)
                {
                    _unitOfWork.ScoreRecordSubject.Remove(s);
                }
                _unitOfWork.RecordSubject.Remove(rs);

            }
            var summaryList = _unitOfWork.SummarySubject.GetAll(x => x.SubjectId == id);
            foreach(var sl in summaryList)
            {
                _unitOfWork.SummarySubject.Remove(sl);
            }
            if (ObjfromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Subject.Remove(ObjfromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }


        #region API CALL
        [HttpGet]

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Subject.GetAll();
            return Json(new { data = allObj });
        }
        #endregion

        [HttpGet]
        public IActionResult Details(int id)
        {
            //.Include(x => x.RecordSubject).ThenInclude(x => x.Average)
            //var subject1 = _db.Subject.Include(x => x.ClassStudent).ThenInclude(x => x.Class)
            //    .Where(x => x.Id == id).SingleOrDefault();

            var subject = _unitOfWork.Subject.GetFirstOrDefault(x => x.Id == id);

            int? passQuantity = 0;
            float? percent = 0;

            var summary = _unitOfWork.SummarySubject.GetFirstOrDefault(x => x.SubjectId == id);
            if(summary != null)
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
    }
}

