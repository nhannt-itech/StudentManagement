using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class ScoreRecordSubjectRepository : Repository<ScoreRecordSubject>, IScoreRecordSubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public ScoreRecordSubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ScoreRecordSubject scoreRecordSubject)
        {
            _db.Update(scoreRecordSubject);
        }
    }
}
