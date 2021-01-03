using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;

namespace StudentManagement.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ResultStatisticController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResultStatisticController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var studentList = _unitOfWork.ClassStudent.GetAll(includeProperties: "Student");
            var recordList = _unitOfWork.RecordSubject.GetAll();

            foreach (var u in studentList)
            {
                u.Student.RecordSubject = _unitOfWork.RecordSubject.GetAll(x => x.StudentId == u.StudentId).ToList();
            }

            foreach (var st in studentList)
            {
                FinalResult finalResult = new FinalResult();
                finalResult.StudentId = st.StudentId;
                finalResult.ClassId = st.ClassId;
                finalResult.AvgSem1 = st.Student.RecordSubject.Where(x => x.Semester == 1 && x.ClassId == st.ClassId).Select(x => x.Average).Average().GetValueOrDefault();
                finalResult.AvgSem2 = st.Student.RecordSubject.Where(x => x.Semester == 2 && x.ClassId == st.ClassId).Select(x => x.Average).Average().GetValueOrDefault();
                finalResult.Final = (finalResult.AvgSem1 + finalResult.AvgSem2)/2;

                var kq = finalResult.Final;

                if(kq >=8 && kq <= 10)
                {
                    finalResult.Rate = "Giỏi";
                }
                else if (kq >=7 && kq <8)
                {
                    finalResult.Rate = "Khá";
                }
                else if (kq >=5 && kq <7)
                {
                    finalResult.Rate = "Trung bình";
                }
                else
                {
                    finalResult.Rate = "Yếu";
                }

                var obj = _unitOfWork.FinalResult.Get(finalResult.StudentId);
                if (obj == null)
                {
                    _unitOfWork.FinalResult.Add(finalResult);
                }
                else
                {
                    _unitOfWork.FinalResult.Update(finalResult);
                }
                _unitOfWork.Save();



            }
            return View();
        }

        public IActionResult StatisticRate()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticRate");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Rate"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }
        public IActionResult StatisticRate10()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticRate10");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Rate"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult StatisticRate11()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticRate11");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Rate"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }
        public IActionResult StatisticRate12()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("StatisticRate12");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Rate"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AverageScore()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AverageScore");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Name"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["TrungBinh"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AverageScore10()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AverageScore10");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Name"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["TrungBinh"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AverageScore11()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AverageScore11");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Name"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["TrungBinh"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AverageScore12()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AverageScore12");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Name"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["TrungBinh"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AreaRate()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AreaRate");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Grade"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AreaRateGioi()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AreaRateGioi");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Grade"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AreaRateKha()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AreaRateKha");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Grade"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }

        public IActionResult AreaRateTB()
        {
            var result = _unitOfWork.SP_Call.ExecuteJson("AreaRateTB");
            if (result.success && result.message != "")
            {
                var objString = result.message;
                var obj = JArray.Parse(objString);
                var labels = obj.Select(x => x["Grade"].ToString()).ToArray();
                var values = obj.Select(x => float.Parse(x["SoLuong"].ToString())).ToArray();
                return Json(new { labels, values });
            }
            else
                return NotFound();
        }


    }
}
