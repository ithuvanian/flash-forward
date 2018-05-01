using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Data.SqlClient;
using Dapper;

namespace Capstone.Web.DAL
{
    public class DeckSqlDAL
    {
        private string connectionString;
        TagsSqlDAL tagsDAL;
        CardSqlDAL cardDAL;

        private string GetAllDecksByUserIDSQL = "SELECT * FROM decks WHERE UserID = @userIDValue ORDER BY DeckID ASC";

        private string GetDeckByDeckIDSQL = "SELECT * FROM decks WHERE DeckID = @deckIDValue ORDER BY DeckID ASC";

        private string GetDecksByNameSQL = "SELECT * FROM decks WHERE UserID = @userIDValue and Name = @nameValue ORDER BY DeckID ASC";

        private string GetDecksByTagSQL = "SELECT * FROM decks " +
            "JOIN deck_tag ON decks.DeckID = deck_tag.DeckID " +
            "WHERE decks.UserID = @userIDValue and deck_tag.TagID = @tagValue ORDER BY decks.DeckID ASC";

        private string GetDecksByCardSQL = "SELECT * FROM decks " +
            "JOIN card_deck ON decks.DeckID = card_deck.DeckID " +
            "WHERE decks.UserID = @userIDValue and card_deck.CardID = @cardIDValue ORDER BY decks.DeckID ASC";

        private string GetAvailableDecksToAddCardSQL = "SELECT DISTINCT DeckID from decks " +
            "WHERE UserID = @userIDValue and DeckID not in (SELECT distinct DeckID from card_deck where CardID = @cardIDValue)";

        private string AddDeckSQL = "INSERT INTO decks (UserID, Name) VALUES (@userIDValue, @nameValue); SELECT CAST(SCOPE_IDENTITY() as int);";

        private string ModifyDeckNameSQL = "UPDATE decks SET Name = @nameValue WHERE DeckID = @deckIDValue;";

        private string ModifyDeckPublicSQL = "UPDATE decks SET IsPublic = @publicValue WHERE DeckID = @deckIDValue;";

        private string AddCardToDeckSQL = "INSERT INTO card_deck (CardID, DeckID) VALUES (@cardIDValue, @deckIDValue);";

        private string RemoveCardFromDeckSQL = "DELETE FROM card_deck WHERE CardID = @cardIDValue AND DeckID = @deckIDValue;";

        public DeckSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
            tagsDAL = new TagsSqlDAL(connectionString);
            cardDAL = new CardSqlDAL(connectionString);
        }

        public List<Deck> GetDecksByUserID(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Query<Deck>(GetAllDecksByUserIDSQL, new { userIDValue = userID });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Deck GetDeckByDeckID(string deckID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(GetDeckByDeckIDSQL, conn);
                    cmd.Parameters.AddWithValue("deckIDValue", deckID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Deck result = new Deck();
                    while (reader.Read())
                    {
                        result.DeckID = Convert.ToString(reader["DeckID"]);
                        result.UserID = Convert.ToString(reader["UserID"]);
                        result.Name = Convert.ToString(reader["Name"]).Trim();
                        result.IsPublic = Convert.ToBoolean(reader["IsPublic"]);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Deck> SearchDecksByName(string userID, string searchName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Query<Deck>(GetDecksByNameSQL, new { userIDValue = userID, nameValue = searchName });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Deck> SearchDecksByTag(string userID, string searchName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Query<Deck>(GetDecksByTagSQL, new { userIDValue = userID, tagValue = searchName });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<Deck> GetDecksByCardID(string userID, string cardID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Query<Deck>(GetDecksByCardSQL, new { userIDValue = userID, cardIDValue = cardID });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Deck> GetAvailableDecksToAddCard(string userID, string cardID)
        {
            List<string> AvailableDeckID = new List<string>();
            List<Deck> resultList = new List<Deck>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Query<string>(GetAvailableDecksToAddCardSQL, new { userIDValue = userID, cardIDValue = cardID });
                    AvailableDeckID = result.ToList();
                }
                foreach (string deckID in AvailableDeckID)
                {
                    resultList.Add(GetDeckByDeckID(deckID));
                }
                return resultList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string AddDeck(string userID, string deckName)
        {
            string newDeckID = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.ExecuteScalar<int>(AddDeckSQL, new { userIDValue = userID, nameValue = deckName });
                    if (result.ToString() != null)
                    {
                        newDeckID = result.ToString();
                    }

                    return newDeckID;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ModifyDeckName(string deckID, string deckName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Execute(ModifyDeckNameSQL, new { deckIDValue = deckID, nameValue = deckName });
                    if (result == 1)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ModifyDeckIsPublic(string deckID, bool isPublic)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Execute(ModifyDeckPublicSQL, new { deckIDValue = deckID, publicValue = isPublic });
                    if (result == 1)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool AddCardToDeck(string cardID, string deckID)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var result = conn.Execute(AddCardToDeckSQL, new { cardIDValue = cardID, deckIDValue = deckID });
                    if (result == 1)
                    {
                        success = true;
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool RemoveCardFromDeck(string cardID, string deckID)
        {
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var result = conn.Execute(RemoveCardFromDeckSQL, new { cardIDValue = cardID, deckIDValue = deckID });
                    if (result == 1)
                    {
                        success = true;
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}