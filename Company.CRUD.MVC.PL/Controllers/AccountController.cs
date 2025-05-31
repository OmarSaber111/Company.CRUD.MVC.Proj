using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.Helpers;
using Company.CRUD.MVC.PL.ViewModels.Auth;

namespace Company.CRUD.MVC.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager
            )
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region SignUp
		//SignUp
		[HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            //Code To Registration

            if (ModelState.IsValid) // Server Side Validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    //Mapping : SignUpViewModel To ApplicationUser

                    user = await _userManager.FindByEmailAsync(model.Email);

                    if (user is null)
                    {

                        user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAgree = model.IsAgree,
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
                    ModelState.AddModelError(string.Empty, "Email Is Already Exist  ");
                    return View(model);

                }
                ModelState.AddModelError(string.Empty, "UserName Is Already Exist  ");
                return View(model);
            }


            return View();
        } 
        #endregion

        #region SignIn
        //SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is not null)
                    {
                        //CheckPassword
                        var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (Flag)
                        {
                            //SignIn                   //isPersistent = true => Gives you 14 day
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            ModelState.AddModelError(string.Empty, "Invalid Login !!");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login !!");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View();
        }

        #endregion
        
        #region SignOut

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    //Create Token 
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Reset Password URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);


                    //https://localhost:44318/Account/SendResetPasswordUrl?email=ahmed@gmail.com&token=

                    //Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    //Send Email

                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again !!");

            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }


		#endregion

		#region Reset Password
		[HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);

                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.ConfirmPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again !!");

            }
            ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again !!");


            return View();
        }

        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
