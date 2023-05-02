using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			if(HttpContext.User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			else
				return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult Logout()
		{
			LoginManager.Get.User = null;
            return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public IActionResult ValidateLogin(LoginForm login)
		{
			if(ModelState.IsValid)
			{
				return View("Index");
			}
			if (DB.SelectBy<User>("Email", login.Email).Count() == 0)
			{
				ModelState.AddModelError("Email", "Uživatel neexistuje");
				return View("Index");
			}
			else
			{
				User a = DB.SelectBy<User>("Email", login.Email).First();
				//var result = signInManager.PasswordSignInAsync(a.Email, a.Password, false, false).Result;
				if (a.Password==login.Password)
				{
					LoginManager.Get.User = a;
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("Password", "Špatné heslo");
					return View("Index");
				}
			}
		}

		[HttpPost]
		public IActionResult ValidateRegister(LoginForm login)
		{
            //if (ModelState.IsValid)
            //{
            //    return View("Register");
            //}
            if (DB.SelectBy<User>("Email", login.Email).Count() == 0)
			{
				if(login.Password != login.PasswordAgain)
				{
					ModelState.AddModelError("PasswordAgain", "Hesla se neshodují");
					return View("Register");
				}
                User a = new User();
                a.Name = login.FirstName;
                a.Surname = login.LastName;
                a.Email = login.Email;
                a.Password = login.Password;
                DB.Insert(a);
				a = DB.SelectBy<User>("Email", login.Email).First();
                LoginManager.Get.User = a;
                return RedirectToAction("Index", "Home");
            }
            else
			{
				ModelState.AddModelError("Email", "Uživatel již existuje");
                return View("Register");
            }
        }

	}
}
