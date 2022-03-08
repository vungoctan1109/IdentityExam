using IdentityExam.Data;
using IdentityExam.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityExam.Controllers
{
    public class UserController : Controller
    {
        private MyIdentityDbContext myIdentityDbContext;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        public UserController()
        {
            myIdentityDbContext = new MyIdentityDbContext();
            UserStore<User> userStore = new UserStore<User>(myIdentityDbContext);
            userManager = new UserManager<User>(userStore);
            RoleStore<Role> roleStore = new RoleStore<Role>(myIdentityDbContext);
            roleManager = new RoleManager<Role>(roleStore);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string Email, string PhoneNumber, string IdentityNumber, int Status, string UserName, string Password)
        {
            User user = new User()
            {
                UserName = UserName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                IdentityNumber = IdentityNumber,
                Status = Status
            };

            var result = await userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                return View("ViewSucceeded");
            }
            else
            {
                return View("ViewError");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string UserName, string Password)
        {
            var user = await userManager.FindAsync(UserName, Password);
            if (user == null)
            {
                return View("ViewError");
            }
            else
            {
                SignInManager<User, string> signInManager = new SignInManager<User, string>(userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(user, false, false);
                return View("ViewSucceeded");
            }
        }

        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/Home");
        }

        public ActionResult GetList()
        {
            var Users = (from user in myIdentityDbContext.Users select user).ToList();
            return View(Users);
        }

        [HttpPost]
        public ActionResult GetList(User model)
        {
            var data = myIdentityDbContext.Users.ToList();
            return View(data);
        }
    }
}