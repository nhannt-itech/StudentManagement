using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class RuleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RuleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update(string id)
        {
            var ruleObj = _unitOfWork.Rule.Get(id);
            return View(ruleObj);
        }

        [HttpPost]
        public IActionResult Update(Rule rule)
        {
            _unitOfWork.Rule.Update(rule);
            _unitOfWork.Save();
            return View("Index");
        }

        #region API CALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Rule.GetAll();
            return Json(new { data = allObj });
        }
        #endregion
    }
}
