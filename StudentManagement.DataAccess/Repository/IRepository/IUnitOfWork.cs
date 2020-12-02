using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }
        void Save();
    }
}
