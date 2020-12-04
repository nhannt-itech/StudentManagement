using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface ISummarySubjectRepository : IRepository<SummarySubject>
    {
        void Update(SummarySubject summarySubject);
    }
}
