using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.BLL.Repositories;
using Company.CRUD.MVC.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.CRUD.MVC.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;

        public UnitOfWork(AppDbContext context)
        {

            _context = context;
            _departmentRepository = new DepartmentRepository(context);
            _employeeRepository = new EmployeeRepository(context);
        }

        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        public IDepartmentRepository DepartmentRepository => _departmentRepository;
    }
}
