using System;
using System.Collections.Generic;
using Capstone.Web.DAL;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Card
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;

        public string CardID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public string UserID { get; set; }
        public string TagName { get; set; }
        /// <summary>
        /// Returns a list of all available Tags.
        /// </summary>
        public List<string> AllTags
        {
            get
            {
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);
                return tagSql.TagList;
            }
        }

        /// <summary>
        /// Returns the Tags associated with an individual card in the current instance.
        /// </summary>
        public List<string> ThisCardTags
        {
            get
            {
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);
                return tagSql.GetTagsByCardID(CardID);
            }
            set { }

        }

        public List<Deck> DecksHaveThisCard(string userID)
        {
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            return deckSql.GetDecksByCardID(userID, CardID);
        }

        public List<Deck> AvailableDecksToAddThisCard(string userID)
        {
                DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
                return deckSql.GetAvailableDecksToAddCard(userID, CardID);
        }

        /// <summary>
        /// Adds a Tag to an individual card in a current instance.
        /// </summary>
        /// <param name="tagName"></param>
        public void AddTagToCard(string tagName)
        {
            TagsSqlDAL tagsSql = new TagsSqlDAL(connectionString);
            tagsSql.AddTagToCard(CardID, tagName);
        }

        public void RemoveTagFromCard(string tagName)
        {
            TagsSqlDAL tagsSql = new TagsSqlDAL(connectionString);
            tagsSql.RemoveTagFromCard(CardID, tagName);
        }
    }
}