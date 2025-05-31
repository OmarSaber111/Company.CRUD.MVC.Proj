using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.ViewModels.Roles;
using Company.CRUD.MVC.PL.ViewModels.User;

namespace Company.CRUD.MVC.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(string searchInput)
        {
            var users = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(searchInput))
            {
                users = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();   
            }
            else
            {
                users = await _roleManager.Roles.Where(U => U.Name
                                  .ToLower()
                                  .Contains(searchInput.ToLower()))
                                  .Select(R => new RoleViewModel
                                  {
                                      Id = R.Id,
                                      RoleName = R.Name
                                  }).ToListAsync();
            }
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName,
                };

               await _roleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest(); //400

            var roleFromDb = await _roleManager.FindByIdAsync(id);

            if (roleFromDb is null) return NotFound(); //404

            var user = new RoleViewModel()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name
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
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);
                    if (roleFromDb is null) return NotFound(); //404

                    roleFromDb.Id= model.Id;
                    roleFromDb.Name = model.RoleName;

                    await _roleManager.UpdateAsync(roleFromDb);
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
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);
                    if (roleFromDb is null) return NotFound(); //404


                    await _roleManager.DeleteAsync(roleFromDb);
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
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            ViewData["RoleName"] = role.Name;
            ViewData["RoleId"] = role.Id;

            if (role is null)
            {
                return NotFound(); //404
            }

            var usersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else 
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }


            return View(usersInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound(); //404
            }

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);

                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                           await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                           await _userManager.RemoveFromRoleAsync(appUser,role.Name);
                        }
                    }
                }

                return RedirectToAction(nameof(Edit), new { id= roleId});

            }

            return View(User);

            
        }
    }
}
