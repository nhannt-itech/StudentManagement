using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public SubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Subject subject)
        {
            _db.Update(subject);
        }
    }
}
