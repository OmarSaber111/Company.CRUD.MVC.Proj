using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.Helpers;
using Company.CRUD.MVC.PL.ViewModels.Employees;
using Company.CRUD.MVC.PL.ViewModels.User;

namespace Company.CRUD.MVC.PL.Controllers
{
    //[Authorize (Roles = "Admin")]
    [Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
		}
		public async Task<IActionResult> Index(string searchInput)
		{
			var users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(searchInput))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult() 
                }).ToListAsync();
			}
			else
			{
				users = await _userManager.Users.Where(U => U.Email
								  .ToLower()
								  .Contains(searchInput.ToLower()))
								  .Select(U=> new UserViewModel
								  {
									  Id = U.Id,
									  FirstName = U.FirstName,
									  LastName = U.LastName,
									  Email = U.Email,
									  Roles = _userManager.GetRolesAsync(U).GetAwaiter().GetResult()
								  }).ToListAsync();
			}  
			return View(users);
		}

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest(); //400

			var userFromDb = await _userManager.FindByIdAsync(id);

			if (userFromDb is null) return NotFound(); //404

			var user = new UserViewModel()
			{
				Id = userFromDb.Id,
				FirstName = userFromDb.FirstName,
				LastName = userFromDb.LastName,
				Email = userFromDb.Email,
				Roles = _userManager.GetRolesAsync(userFromDb).GetAwaiter().GetResult()
			};



            return View(viewName, user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();  
                if (ModelState.IsValid)
                {
                   var userFromDb = await _userManager.FindByIdAsync(id);
                    if (userFromDb is null) return NotFound(); //404

                    userFromDb.FirstName = model.FirstName;
                    userFromDb.LastName = model.LastName;
                    userFromDb.Email = model.Email;

                    await _userManager.UpdateAsync(userFromDb);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();  
                if (ModelState.IsValid)
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);
                    if (userFromDb is null) return NotFound(); //404

                   
                    await _userManager.DeleteAsync(userFromDb);
                    return RedirectToAction("Index");
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
