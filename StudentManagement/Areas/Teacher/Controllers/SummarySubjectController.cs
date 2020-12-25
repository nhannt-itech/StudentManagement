using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class SummarySubjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SummarySubject(string? classId,int? semeter)
        {

            return Json("hi");
        }
    }
}
