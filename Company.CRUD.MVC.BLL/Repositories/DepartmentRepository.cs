using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.DAL.Data.Contexts;
using Company.CRUD.MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.CRUD.MVC.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {


        public DepartmentRepository(AppDbContext context) :base(context) //ASK CLR Create Object From AppDbContext
        {

        }

      

    }
}
