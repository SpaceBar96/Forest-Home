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
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegisterDetails(Register rgstr)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new ForestEntities())
                {
                    User reglog = new User();

                    reglog.UserID = Guid.NewGuid();
                    reglog.Name = rgstr.Name;
                    reglog.Email = rgstr.Email;
                    reglog.Password = PasswordEncrypt.TextToEncrypt(rgstr.Password);

                    databaseContext.Users.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {
                return View("Register", rgstr);
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
            using (var dataContext = new ForestEntities())
            {
                var _passWord = PasswordEncrypt.TextToEncrypt(model.Password);
                User user = dataContext.Users.Where(query => query.Email == model.Email && query.Password == _passWord).SingleOrDefault();

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
            return RedirectToAction("Index");
            }

        }
    }
