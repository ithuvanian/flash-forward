using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using System.Configuration;
using Capstone.Web.DAL;

namespace Capstone.Web.Controllers
{
    public class AdminController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        private CardSqlDAL cDal = new CardSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private DeckSqlDAL dDal = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private UserSqlDAL uDal = new UserSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private TagsSqlDAL tDal = new TagsSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);

        //Let's the admin view all decks
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("You are not authorized to view this page.");
            }
            string isAdmin = Session["admin"].ToString();
            //List<Deck> decks = dDal.AdminGetAllDecks();

            return View("Home", "Index");
        }

        //GET: Approve a Deck for public use
        public ActionResult ApproveDeck(string deck_id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("You are not authorized to perfom this function");
            }
            string isAdmin = Session["admin"].ToString();

            Deck curDeck = dDal.GetDeckByDeckID(deck_id);
            curDeck.MakePublic(deck_id);

            return View("Deck");
        }

        //Delete a Deck
        [HttpPost]
        public ActionResult DeleteDeck(string deck_id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("You are not authorized to perfom this function");
            }
            string isAdmin = Session["admin"].ToString();

            Deck curDeck = dDal.GetDeckByDeckID(deck_id);
            //curDeck.AdminDeleteDeck(deck_id);
            return RedirectToAction("Deck");
        }

        // GET: Delete a Tag
        [HttpPost]
        public ActionResult DeleteTag(string deck_id, string tagName)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("You are not authorized to perfom this function");
            }
            string isAdmin = Session["admin"].ToString();

            Deck curDeck = dDal.GetDeckByDeckID(deck_id);
            curDeck.AdminDeleteTag(tagName);
            return RedirectToAction("Deck");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}