using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.Helpers;
using Company.CRUD.MVC.PL.ViewModels.Employees;

namespace Company.CRUD.MVC.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

   
        public async Task<IActionResult> Index(string searchInput)
        {
            var employee = Enumerable.Empty<Employee>(); 

            if (string.IsNullOrEmpty(searchInput))
            {
                // EF Core : Don't Loading The Navigational Property
               employee = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employee = await _unitOfWork.EmployeeRepository.GetByNameAsync(searchInput);
            }


            //Auto Mapping
            
            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employee);

            //string Message = "Hello World";

            //// Key --- Value
            //// Message :

            //// View's Dictionary : [Extra InFormation] Transfer Data From Action To View  [One Way]

            //// 1 - ViewData : Property Inherited From Controller - Dictionary

            //ViewData["Message"] = Message + " From ViewData";

            //// 2 - ViewBag  : Property Inherited From Controller - dynamic

            //ViewBag.Hamada = Message + " From ViewBag";

            //// 3 - TempData : Property Inherited From Controller - Dictionary
            //// Transfer For The Data From Request To Another  

            //TempData["Message01"] = Message + " From TempData";

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information

            //View's Dictionary :
            // 1. ViewData

            ViewData["Departments"] = departments;

            // 2. ViewBag
            // 3. TempData

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
          
            if (ModelState.IsValid) // Server Side Validation
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image,"images");
                //Casting : EmployeeViewModel -> Employee
                //Manual Mapping 
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    Salary = model.Salary,
                //    PhoneNumber = model.PhoneNumber,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    DateOfCreation = model.DateOfCreation,  
                //    HiringDate = model.HiringDate,
                //    WorkForId = model.WorkForId,
                //    WorkFor = model.WorkFor 

                //};


                //Automatic Mapping

                var employee = _mapper.Map<Employee>(model);

                // Insert Department to Database
                var count = await _unitOfWork.EmployeeRepository.AddAsync(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created Successfully";
                }
                else
                {
                    TempData["Message"] = "Employee is Not Created Successfully";
                }
                    return RedirectToAction(nameof(Index));
            }
           
            return View();
        }

        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if(id is null) return BadRequest(); //400
             
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if(employee is null) return NotFound(); //404

            var result = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewName,result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information

            //View's Dictionary :
            // 1. ViewData

            ViewData["Departments"] = departments;

            // 2. ViewBag
            // 3. TempData

            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    if(model.Image is not null)
                    {

                        if (model.ImageName is not null)
                        {
                            DocumentSettings.DeleteFile(model.ImageName, "images");
                        }
                        model.ImageName = DocumentSettings.UploadFile(model.Image , "images");

                    }
                    else
                    {
                        model.ImageName = model.ImageName;
                    }


                    //Manual Mapping
                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Address = model.Address,
                    //    Salary = model.Salary,
                    //    PhoneNumber = model.PhoneNumber,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    DateOfCreation = model.DateOfCreation,
                    //    HiringDate = model.HiringDate,
                    //    WorkForId = model.WorkForId,
                    //    WorkFor = model.WorkFor

                    //};

                    //Automatic Mapping
                    var employee = _mapper.Map<Employee>(model);


                    var count = await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            try
            { 
                //Manual Mapping
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    Salary = model.Salary,
                //    PhoneNumber = model.PhoneNumber,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    DateOfCreation = model.DateOfCreation,
                //    HiringDate = model.HiringDate,
                //    WorkForId = model.WorkForId,
                //    WorkFor = model.WorkFor

                //};

                //Automatic Mapping
                var employee = _mapper.Map<Employee>(model);


                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var count = await _unitOfWork.EmployeeRepository.DeleteAsync(employee);
                    if (count > 0)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images"); 
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return View();
        }



    }
}
