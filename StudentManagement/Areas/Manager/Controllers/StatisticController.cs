using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Utility;

namespace StudentManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.Role_Manager)]
    public class StatisticController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //Tổng SL học sinh toàn trường
            ViewBag.sumHS = _unitOfWork.Student.GetAll().Count();

            //Tổng số lượng HS khối 10
            var objList10 = _unitOfWork.Class.GetAll(x => x.Grade == 10);
            ViewBag.sumHS10 = objList10.Sum(x => x.NumStudents);

            //Tổng số lượng HS khối 11
            var objList11 = _unitOfWork.Class.GetAll(x => x.Grade == 11);
            ViewBag.sumHS11 = objList11.Sum(x => x.NumStudents);

            //Tổng số lượng HS khối 12
            var objList12 = _unitOfWork.Class.GetAll(x => x.Grade == 12);
            ViewBag.sumHS12 = objList12.Sum(x => x.NumStudents);

            //Tổng SL môn học
            ViewBag.sumSubject = _unitOfWork.Subject.GetAll().Count();

            //Tổng SL lớp học
            ViewBag.sumClass = _unitOfWork.Class.GetAll().Count();
            return View();
        }

        public IActionResult StatisticGender()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticGender");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Gender"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult StatisticGender10()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticGender10");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Gender"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult StatisticGender11()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticGender11");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Gender"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }


        public IActionResult StatisticGender12()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticGender12");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Gender"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }
    }
}
