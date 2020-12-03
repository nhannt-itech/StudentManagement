using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IClassRepository Class { get; }
        IClassStudentRepository ClassStudent { get; }
        IRecordSubjectRepository RecordSubject { get; }
        IScoreRecordSubjectRepository ScoreRecordSubject { get; }
        IStudentRepository Student { get; }
        ISummarySubjectRepository SummarySubject { get; }
        ISummarySubjectSemeterRepository SummarySubjectSemeter { get; }

        ISP_Call SP_Call { get; }
        void Save();
    }
}
