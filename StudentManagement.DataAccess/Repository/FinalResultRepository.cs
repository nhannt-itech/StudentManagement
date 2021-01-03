using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class FinalResultRepository : Repository<FinalResult>, IFinalResultRepository
    {
        private readonly ApplicationDbContext _db;
        public FinalResultRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FinalResult finalResult)
        {
            var objFromDb = _db.FinalResult.FirstOrDefault(s => s.StudentId == finalResult.StudentId);
            if (objFromDb != null)
            {
                objFromDb.AvgSem1 = finalResult.AvgSem1;
                objFromDb.AvgSem2 = finalResult.AvgSem2;
                objFromDb.Final = finalResult.Final;
                objFromDb.Rate = finalResult.Rate;
            }
        }
    }
}
