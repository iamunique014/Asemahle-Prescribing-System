using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiphoAuth.Controllers
{
    public class AccountMphoController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountMphoController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("CustomerCare", "Customer");

                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser users = new ApplicationUser
                {
                FirstName= model.Name,
                LastName=model.LastName,
                IdentityNumber= model.IdentityNumber,
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await userManager.CreateAsync(users, model.Password);

            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(model);
            }
                
            }
            return View(model);
        }
     
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmail model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "wrong Email/Username");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new {username = user.UserName});
                }
            }
            return View(model);
        }
     
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username});
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync (model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if(result.Succeeded) 
                    {

                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("","Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "something is wrong. Try Again.");
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
