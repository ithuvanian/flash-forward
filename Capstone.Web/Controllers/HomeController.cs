using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.DAL;
using System.Configuration;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {


        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        // GET: Home
        public ActionResult Index()
        {
            //temporary user id
            //Session["userid"] = "7";

            Session["anon"] = "Home";
            return View("Index");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            UserSqlDAL userDal = new UserSqlDAL(connectionString);

            User user = userDal.GetUser(model.Email);

            if (user.Email == null || user.Password != model.Password)
            {
                ModelState.AddModelError("invalid-credentials", "An invalid email or password was provided");
                return View("Login", model);
            }

            //if user clicked on 'cards' or 'decks' before logging in, take them there now
            Session["userid"] = user.Id;
            Session["admin"] = user.IsAdmin;
            switch (Session["anon"].ToString())
            {
                case "Cards":
                    return RedirectToAction("Index", "Card");
                case "Decks":
                    return RedirectToAction("Index", "Deck");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session["anon"] = "Home";
            return View("Logout");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            UserSqlDAL newUserDAL = new UserSqlDAL(connectionString);
            //attempt to retrieve provided email - cannot duplicate existing
            User newUser = newUserDAL.GetUser(model.Email);
            if (newUser.Email == null)
            {
                newUser.Email = model.Email;
                newUser.Password = model.Password;
                if (model.DisplayName == null)
                {
                    newUser.DisplayName = model.Email.Substring(0, model.Email.IndexOf('@'));
                }
                else
                {
                    newUser.DisplayName = model.DisplayName;
                }
                
                newUserDAL.Register(newUser);
                User retriveUser = newUserDAL.GetUser(newUser.Email);

                Session["userid"] = retriveUser.Id;
                Session["admin"] = retriveUser.IsAdmin;
            }
            else
            {
                ModelState.AddModelError("email-exists", "That email address exists, please contact Admin for password reset if needed.");
                return View("Register", model);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}