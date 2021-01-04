using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models.ViewModels;
using StudentManagement.Utility;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = SD.Role_Teacher + "," + SD.Role_Manager)]
    public class SummarySubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummarySubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            SummarySubjectVM summarySubjectVM = new SummarySubjectVM()
            {
                YearList = _unitOfWork.Class.GetAll().Select(x => x.Year).Distinct()
                                    .Select(i => new SelectListItem
                                    {
                                        Text = i,
                                        Value = i
                                    }),
                SubjectList = _unitOfWork.Subject.GetAll()
                                    .Select(i => new SelectListItem
                                    {
                                        Text = i.Name,
                                        Value = i.Id.ToString()
                                    })
            };
            return View(summarySubjectVM);
        }

        [HttpPost]
        public IActionResult Index(int subjectId, int grade, string year, int semester)
        {
             var allObj = _unitOfWork.SummarySubject.GetAll(x => x.SubjectId == subjectId &&
                                                                        x.Class.Grade == grade &&
                                                                        x.Class.Year == year &&
                                                                        x.Semester == semester, includeProperties: "Class");
            foreach(var item in allObj)
            {
                item.PassQuantity = _unitOfWork.RecordSubject.GetAll().Count(x => x.ClassId == item.ClassId &&
                                                                                        x.Semester == item.Semester &&
                                                                                        x.SubjectId == item.SubjectId &&
                                                                                        x.Average >= _unitOfWork.Rule.GetFirstOrDefault(x => x.Name == "Điểm chuẩn").Min);
                if (_unitOfWork.ClassStudent.GetAll().Count(x => x.ClassId == item.ClassId) != 0)
                {
                    item.Percentage = (float)item.PassQuantity / (float)_unitOfWork.ClassStudent.GetAll().Count(x => x.ClassId == item.ClassId);
                }
                else
                {
                    item.Percentage = 0;
                }
                _unitOfWork.SummarySubject.Update(item);
                _unitOfWork.Save();
                // coi chừng khúc này nếu chưa có học sinh.
            }
            return Json(new { data = allObj });
        }
    }
}
