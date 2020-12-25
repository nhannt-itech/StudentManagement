using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        private readonly ApplicationDbContext _db;
        public ClassRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Class @class)
        {
            _db.Update(@class);
        }
    }
}
