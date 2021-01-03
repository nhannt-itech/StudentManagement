using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Class = new ClassRepository(_db);
            ClassStudent = new ClassStudentRepository(_db);
            RecordSubject = new RecordSubjectRepository(_db);
            ScoreRecordSubject = new ScoreRecordSubjectRepository(_db);
            Student = new StudentRepository(_db);
            Subject = new SubjectRepository(_db);
            Summary = new SummaryRepository(_db);
            SummarySubject = new SummarySubjectRepository(_db);
            Rule = new RuleRepository(_db);
            FinalResult = new FinalResultRepository(_db);
            SP_Call = new SP_Call(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IClassRepository Class { get; private set; }
        public IClassStudentRepository ClassStudent { get; private set; }
        public IRecordSubjectRepository RecordSubject { get; private set; }
        public IScoreRecordSubjectRepository ScoreRecordSubject { get; private set; }
        public IStudentRepository Student { get; private set; }
        public ISubjectRepository Subject { get; private set; }
        public ISummaryRepository Summary { get; private set; }
        public ISummarySubjectRepository SummarySubject { get; private set; }
        public IRuleRepository Rule { get; private set; }
        public IFinalResultRepository FinalResult { get; private set; }


        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
