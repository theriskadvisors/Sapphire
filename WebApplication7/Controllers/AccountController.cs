using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class AccountController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            var username = Request.Form["email"];
            var pass = Request.Form["password"];
            var userlogin = db.Accounts.Where(x => x.Email == username && x.Password == pass).FirstOrDefault();
            if (userlogin != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("AdminDashboard", "Home");
            }


        }
        public ActionResult AdminDashboard()
        {
            return View();
        }
    }
}