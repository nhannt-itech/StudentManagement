using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IClassStudentRepository : IRepository<ClassStudent>
    {
        void Update(ClassStudent classStudent);
    }
}
