using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class SummaryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryController(IUnitOfWork unitOfWork)
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
                                       })
            };
            return View(summarySubjectVM);
        }

        [HttpPost]
        public IActionResult Index(int grade, string year, int semester)
        {
            var allObj = _unitOfWork.Summary.GetAll(x => x.Class.Grade == grade &&
                                                                       x.Class.Year == year &&
                                                                       x.Semeter == semester, includeProperties: "Class");
            foreach (var item in allObj)
            {
                var passAllObj = _unitOfWork.RecordSubject.GetAll().GroupBy(x => new { x.ClassId, x.StudentId })
                                                            .Select(g => new
                                                            {
                                                                Key = g.Key,
                                                                AverageTotal = g.Average(p => p.Average),
                                                            }).Where(x => x.AverageTotal >= 5);
                item.PassQuantity = passAllObj.Count(x => x.Key.ClassId == item.ClassId);

                if (_unitOfWork.ClassStudent.GetAll().Count(x => x.ClassId == item.ClassId) != 0)
                {
                    item.Percentage = (float)item.PassQuantity / (float)_unitOfWork.ClassStudent.GetAll().Count(x => x.ClassId == item.ClassId);
                }
                else
                {
                    item.Percentage = 0;
                }
                _unitOfWork.Summary.Update(item);
                _unitOfWork.Save();
            }
            return Json(new { data = allObj });
        }
    }
}
