using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.DAL;
using Capstone.Web.Models;
using System.Configuration;

namespace Capstone.Web.Controllers
{


    public class CardController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        private CardSqlDAL cDal = new CardSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private DeckSqlDAL dDal = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);


        // GET: Card
        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                Session["anon"] = "Cards";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        //create new card
        public ActionResult CardConstruct()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            TagsSqlDAL tDal = new TagsSqlDAL(connectionString);
            ViewBag.allTags = tDal.TagDictionary;
            return View("CardCreate");
        }

        //add created card
        public ActionResult CardSubmit(Card newCard)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            cDal.CreateCard(newCard, Session["userid"].ToString());

            List<Card> allCards = cDal.ViewCards(Session["userid"].ToString());

            return View("CardView", allCards);
        }

        //search cards by tag
        public ActionResult CardSearch(string searchString)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (String.IsNullOrEmpty(searchString))
            {
                return View("Index");
            }
            else
            {
                List<Card> matchingCards = cDal.SearchCard(searchString);

                return View("CardSearch", matchingCards);
            }
        }

        //view all user cards
        public ActionResult CardView()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<Card> allCards = cDal.ViewCards(Session["userid"].ToString());

            return View("CardView", allCards);
        }

        //view all user cards
        public ActionResult CardViewWithAdmin()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<Card> allCardsWithAdmin = cDal.ViewCardsWithAdminCards(Session["userid"].ToString());

            return View("CardView", allCardsWithAdmin);
        }

        //modify cards
        public ActionResult CardModify(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card existingCard = cDal.GetCardByID(id);
            if (existingCard.UserID != Session["userid"].ToString())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("CardModify", existingCard);
            }
        }

        public ActionResult CardModifyExpanded(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card existingCard = cDal.GetCardByID(id);
            if (existingCard.UserID != Session["userid"].ToString())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("CardModifyExpanded", existingCard);
            }
        }

        public ActionResult TagExpand(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card existingCard = cDal.GetCardByID(id);
            if (existingCard.UserID != Session["userid"].ToString())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("CardModifyExpanded", existingCard);
            }
        }

        public ActionResult TagCollapse(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card existingCard = cDal.GetCardByID(id);
            if (existingCard.UserID != Session["userid"].ToString())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("CardModify", existingCard);
            }
        }


        //card tags
        [HttpPost]
        public ActionResult AddCardTag(string cardID, string tagName, bool expanded)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card currentCard = cDal.GetCardByID(cardID);
            currentCard.AddTagToCard(tagName);
            if (expanded)
            {
                return RedirectToAction(currentCard.CardID, "Card/CardModifyExpanded");
            }
            return RedirectToAction(currentCard.CardID, "Card/CardModify");
        }

        [HttpPost]
        public ActionResult RemoveCardTag(string cardID, string tagName, bool expanded)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card currentCard = cDal.GetCardByID(cardID);
            currentCard.TagName = tagName;
            currentCard.RemoveTagFromCard(tagName);
            if (expanded)
            {
                return RedirectToAction(currentCard.CardID, "Card/CardModifyExpanded");
            }
            return RedirectToAction(currentCard.CardID, "Card/CardModify");
        }

        [HttpPost]
        public ActionResult CreateCardTag(Card model)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card currentCard = cDal.GetCardByID(model.CardID);

            //if empty input is submitted
            if (model.TagName == null)
            {
                return View("CardModify", model);
            }
            //makes all tags lowercase to avoid conflicts
            model.TagName = model.TagName.ToLower();

            model.AddTagToCard(model.TagName);
            return RedirectToAction(model.CardID, "Card/CardModify");
        }


        public ActionResult CardEditFields(string id, string front, string back, List<string> tags)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Card currentCard = new Card();
            currentCard.CardID = id;
            currentCard.Front = front;
            currentCard.Back = back;
            currentCard.ThisCardTags = tags;
 
            cDal.EditCard(currentCard);

            List<Card> allCards = cDal.ViewCards(Session["userid"].ToString());

            return RedirectToAction(currentCard.CardID, "Card/CardModify");
        }


        public ActionResult CardToDeck(string cardID)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Card currentCard = cDal.GetCardByID(cardID);
            ViewBag.CardID = cardID;
            ViewBag.CurCard = currentCard;

            string userID = Session["userid"].ToString();

            List<string> PublicUserCardList = cDal.PublicCardUserList;

            List<Deck> allDecks = dDal.GetDecksByUserID(userID);

            if (currentCard.UserID != Session["userid"].ToString() && !PublicUserCardList.Contains(currentCard.UserID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("CardToDeck", allDecks);
            }
        }

        public ActionResult AddCardToDeck(string cardID, string deckID)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            dDal.AddCardToDeck(cardID, deckID);

            Card currentCard = cDal.GetCardByID(cardID);
            TempData["addedCard_ID"] = cardID;

            return RedirectToAction("EditDeck", "Deck", new { id = deckID });
        }
    }
}