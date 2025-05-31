using AutoMapper;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.ViewModels.Employees;

namespace Company.CRUD.MVC.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
