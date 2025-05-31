using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.DAL.Models;

namespace Company.CRUD.MVC.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository; //NULL 
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork) //ASK CLR To Create Object From Department Repository
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                var Count = await _unitOfWork.DepartmentRepository.AddAsync(model);
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();

        }

        
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest(); //400

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (!(department is not null)) return NotFound(); //404

            return View(viewName,department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) return BadRequest(); //400

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(); //404

            //return View(department);

            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int? id, Department model)
        {
            try
            {
                if (id != model.Id) return BadRequest(); //400
                if (ModelState.IsValid)
                {
                    var Count = await _unitOfWork.DepartmentRepository.UpdateAsync(model);
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty,Ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest(); //400

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(); //404

            //return View(department);
            return await Details(id, "Delete");
        }

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Department model)
        {
            try
            {
                if (id != model.Id) return BadRequest(); //400
                if (ModelState.IsValid) //Server Side Validation
                {
                    var Count = await _unitOfWork.DepartmentRepository.DeleteAsync(model);
                    if (Count > 0)
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



    }
}
