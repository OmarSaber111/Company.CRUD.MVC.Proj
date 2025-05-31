using Company.CRUD.MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.CRUD.MVC.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {

        Task<IEnumerable<Employee>> GetByNameAsync(string name);
        ////Additional methods
        //Employee GetByName(string Name);


        //IEnumerable<Employee> GetAll();
        //Employee Get(int id);
        //int Add(Employee entity);
        //int Update(Employee entity);
        //int Delete(Employee entity);
        
    }
}
