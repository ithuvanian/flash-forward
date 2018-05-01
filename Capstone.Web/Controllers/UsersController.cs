//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Capstone.Web.Models;
//using System.Configuration;

//namespace Capstone.Web.Controllers
//{
//    public class UsersController : Controller
//    {
//        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;

//        private AdminContext db = new AdminContext();

//        // GET: Users
//        public ActionResult Index()
//        {
//            return View("Index");
//        }

//        // GET: Users/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    User user = db.Users.Find(id);
//        //    if (user == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(user);
//        //}

//        // GET: Users/Create
//        public ActionResult CreateDeck()
//        {
//            return View();
//        }
//        //Add new deck
//        //[HttpGet]
//        //public ActionResult AddDeck()
//        //{
//        //    if (Session["userid"] == null)
//        //    {
//        //        return RedirectToAction("Login", "Home");
//        //    }
//        //    return View("NewDeck");
//        //}


//        // POST: Users/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        //[ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Email,Password,ConfirmPassword,IsAdmin,DisplayName")] User user)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Users.Add(user);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(user);
//        }

//        // GET: Users/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        // POST: Users/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        //[ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Email,Password,ConfirmPassword,IsAdmin,DisplayName")] User user)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(user).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(user);
//        }

//        //GET: Approve a Deck
//        public ActionResult ApproveDeck(int deck_id)
//        {

//        }

//        // GET: Delete a Deck
//        public ActionResult DeleteDeck(int? deck_id)
//        {
//            if (deck_id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(deck_id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        [HttpPost, ActionName("Delete")]
//        //[ValidateAntiForgeryToken]
//        public ActionResult DeleteDeckConfirmed(int deck_id)
//        {
//            User user = db.Users.Find(deck_id);
//            db.Users.Remove(user);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        // GET: Delete a Deck
//        public ActionResult DeleteTag(int? tag_id)
//        {
//            if (tag_id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            User user = db.Users.Find(tag_id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

//        [HttpPost, ActionName("Delete")]
//        //[ValidateAntiForgeryToken]
//        public ActionResult DeleteTagConfirmed(int tag_id)
//        {
//            User user = db.Users.Find(tag_id);
//            db.Users.Remove(user);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
