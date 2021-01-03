using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Repository.IRepository;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.DataAccess.Repository
{
    public class RuleRepository : Repository<Rule>, IRuleRepository
    {
        private readonly ApplicationDbContext _db;
        public RuleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Rule rule)
        {
            _db.Update(rule);
        }
    }
}
