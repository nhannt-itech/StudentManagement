using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IClassStudentRepository ClassStudent { get; }
        IClassRepository Class { get; }
        IRecordSubjectRepository RecordSubject { get; }
        IScoreRecordSubjectRepository ScoreRecordSubject { get; }
        IStudentRepository Student { get; }
        ISubjectRepository Subject { get; }
        ISummaryRepository Summary { get; }
        ISummarySubjectRepository SummarySubject { get; }

        ISP_Call SP_Call { get; }
        void Save();
    }
}
