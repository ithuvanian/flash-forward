using System;
using System.Collections.Generic;
using System.Configuration;
using Capstone.Web.DAL;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Deck
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        private CardSqlDAL cardDAL = new CardSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private TagsSqlDAL tagsDAL = new TagsSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);
        private DeckSqlDAL deckDAL = new DeckSqlDAL(ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString);

        public string DeckID { get; set; }

        public string UserID { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public string TagName { get; set; }

        public string CardID { get; set; }

        /// <summary>
        /// Returns a List<Card> of all cards in this instance of deck. Requires DeckID to not be null.
        /// </summary>
        public List<Card> DeckCards
        {
            get
            {
                return cardDAL.ViewCardsInDeck(DeckID);
            }
        }

        /// <summary>
        /// Returns a list of all available Tags.
        /// </summary>
        public List<string> AllTags
        {
            get
            {
                return tagsDAL.TagList;
            }
        }

        /// <summary>
        /// Returns a List<SelectListItem> of all available tags.
        /// </summary>
        public List<SelectListItem> AllTagsSelectList
        {
            get
            {
                return tagsDAL.TagSelectItemList;
            }
        }

        /// <summary>
        /// Returns the Tags associated with an individual deck in the current instance.
        /// </summary>
        public List<string> ThisDeckTags
        {
            get
            {
                return tagsDAL.GetTagsByDeckID(DeckID);
            }
        }

        /// <summary>
        /// Adds a Tag to an individual deck in a current instance.
        /// </summary>
        /// <param name="tagName"></param>
        public void AddTagToDeck(string tagName)
        {
            tagsDAL.AddTagToDeck(DeckID, tagName);
        }

        public void RemoveTagFromDeck(string tagName)
        {
            tagsDAL.RemoveTagFromDeck(DeckID, tagName);
        }

        public bool AddCardToDeck(string cardID)
        {
            return deckDAL.AddCardToDeck(cardID, DeckID);
        }

        public bool RemoveCardFromDeck(string cardID)
        {
            return deckDAL.RemoveCardFromDeck(cardID, DeckID);
        }

        //public void AdminDeleteDeck(string DeckID)
        //{
        //    deckDAL.AdminDeleteDeck(DeckID);
        //}

        public void AdminDeleteTag(string TagName)
        {
            tagsDAL.AdminDeleteTag(TagName);
        }

        public bool MakePublic(string DeckID)
        {
            return deckDAL.ModifyDeckIsPublic(DeckID, true);
        }
    }
}