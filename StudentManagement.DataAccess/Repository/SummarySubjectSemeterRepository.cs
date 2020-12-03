using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class SummarySubjectSemeterRepository : Repository<SummarySubjectSemeter>, ISummarySubjectSemeterRepository
    {
        private readonly ApplicationDbContext _db;
        public SummarySubjectSemeterRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SummarySubjectSemeter summarySubjectSemeter)
        {
            _db.Update(summarySubjectSemeter);
        }
    }
}
