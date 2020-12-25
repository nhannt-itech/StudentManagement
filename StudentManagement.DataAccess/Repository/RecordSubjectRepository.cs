using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class RecordSubjectRepository : Repository<RecordSubject>, IRecordSubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public RecordSubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RecordSubject recordSubject)
        {
            _db.Update(recordSubject);
        }
    }
}
