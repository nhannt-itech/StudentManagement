using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class SummarySubjectRepository : Repository<SummarySubject>, ISummarySubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public SummarySubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SummarySubject summarySubject)
        {
            _db.Update(summarySubject);
        }
    }
}
