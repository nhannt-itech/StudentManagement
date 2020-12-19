using System;
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
    public class SearchScoreController : Controller
    {
        private readonly ILogger<SearchScoreController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public static string ClassId; //id of class

        public SearchScoreController(ILogger<SearchScoreController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            var classList = _unitOfWork.Class.GetAll();
            return View(classList);
        }
        public IActionResult ScoreList(string id) //id của class
        {
            var studentList = _unitOfWork.Student.GetAll(includeProperties: "RecordSubject");
            ViewBag.lop = _unitOfWork.Class.Get(id).Name.ToString();
            var searchScoreList = new List<SearchScoreVM>();
            foreach(var st in studentList)
            {
                SearchScoreVM score = new SearchScoreVM();
                score.Student = st;
                score.AvgSem1 = st.RecordSubject.Where(x=>x.Semeter ==1 && x.ClassId == id).Select(x => x.Average).Average().GetValueOrDefault();
                score.AvgSem2 = st.RecordSubject.Where(x => x.Semeter == 2 && x.ClassId == id).Select(x => x.Average).Average().GetValueOrDefault();
                searchScoreList.Add(score);
            }
            return View(searchScoreList);
        }
    }
}
