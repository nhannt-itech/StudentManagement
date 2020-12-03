using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface ISummarySubjectSemeterRepository : IRepository<SummarySubjectSemeter>
    {
        void Update(SummarySubjectSemeter summarySubjectSemeter);
    }
}
