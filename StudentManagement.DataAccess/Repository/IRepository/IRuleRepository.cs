using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IRuleRepository : IRepository<Rule>
    {
        void Update(Rule rule);
    }
}
