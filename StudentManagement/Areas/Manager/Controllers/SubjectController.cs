using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Manager")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}

