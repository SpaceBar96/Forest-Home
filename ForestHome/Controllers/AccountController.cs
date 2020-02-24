using ForestHome.DBModel;
using ForestHome.Models;
using ForestHome.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ForestHome.Controllers
{
    public class AccountController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register() //get the email verification /forget password /add Date time
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegisterDetails(Register rgstr)
        {
            using (var databaseContext = new ForestEntities1())

                if (databaseContext.Users.Any(x => x.Email == rgstr.Email))
                {
                    ViewBag.DuplicateMessage = "Email already exist";
                    return View("Register");
                }
            using (var databaseContext = new ForestEntities1())
            {
                if (ModelState.IsValid)
                {
                    User reglog = new User();

                    reglog.UserID = Guid.NewGuid();
                    reglog.Name = rgstr.Name;
                    reglog.Email = rgstr.Email;
                    reglog.Password = PasswordEncrypt.TextToEncrypt(rgstr.Password);

                    databaseContext.Users.Add(reglog);
                    databaseContext.SaveChanges();


                    ViewBag.Message = "YOU HAVE REGISTER SUCCESSFULLY";
                    return View("Register");
                }
                else
                {
                    return View("Register", rgstr);
                }

            }
        }
            public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var isValidUser = IsValidUser(model);

                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else  
                {
                    ModelState.AddModelError("Failure", "Wrong Username and Password combination");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }
        public User IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new ForestEntities1())
            {
                var _passWord = PasswordEncrypt.TextToEncrypt(model.Password);
                User user = dataContext.Users.Where(X => X.Email == model.Email && X.Password == _passWord).SingleOrDefault();

                if (user == null)
                    return null;

                else
                    return user;
            }
}
            public ActionResult LogOut()
            {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Home");
            }

        }
    }
