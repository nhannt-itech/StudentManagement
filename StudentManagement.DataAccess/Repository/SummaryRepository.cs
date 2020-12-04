using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class SummaryRepository : Repository<Summary>, ISummaryRepository
    {
        private readonly ApplicationDbContext _db;
        public SummaryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Summary summary)
        {
            _db.Update(summary);
        }
    }
}
