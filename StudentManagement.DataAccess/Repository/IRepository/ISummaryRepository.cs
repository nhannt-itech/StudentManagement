using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    
    public interface ISummaryRepository : IRepository<Summary>
    {
        void Update(Summary summary);
    }
}
