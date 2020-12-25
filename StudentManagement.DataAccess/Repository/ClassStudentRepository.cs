using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class ClassStudentRepository : Repository<ClassStudent>, IClassStudentRepository
    {
        private readonly ApplicationDbContext _db;
        public ClassStudentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ClassStudent classStudent)
        {
            _db.Update(classStudent);
        }
    }
}
