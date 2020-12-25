using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IClassRepository : IRepository<Class>
    {
        void Update(Class @class);
    }
}
