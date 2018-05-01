using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public class CardSqlDAL
    {
        private string connectionString;


        //private string view_cards = "SELECT * FROM [cards] ORDER BY CardID";

        private string view_cards_by_userID = "SELECT * FROM [cards] WHERE UserID = @user_id";

        private string view_cards_in_deck = "SELECT * FROM cards JOIN card_deck ON cards.CardID = card_deck.CardID JOIN decks ON decks.DeckID = card_deck.DeckID WHERE decks.DeckID = @deck_id";

        private string get_card_by_cardID = "SELECT * FROM [cards] WHERE CardID = @card_id";

        private string create_card = "INSERT INTO [cards] (Front, Back, UserID)" +
           "VALUES (@front, @back, @user_id);";

        private string edit_Card = "UPDATE [cards] SET Front = @front, Back = @back WHERE CardID = @id";

        private string search_Card = "SELECT * FROM[cards] JOIN card_tag ON cards.CardID = card_tag.CardID JOIN tags on card_tag.TagID = tags.TagID WHERE[TagName] = @TagName";

        private string getAdminListSQL = "SELECT UserID FROM users WHERE IsAdmin = 1;";
        private string view_cards_by_userID_with_Admin = "SELECT * FROM [cards] WHERE UserID IN (SELECT value FROM STRING_SPLIT(@userSplitValue, ','));";

        public CardSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<string> PublicCardUserList
        {
            get
            {
                return PublicUserList();
            }
        }

        //List all cards that a user has
        public List<Card> ViewCards(string userID)
        {
            List<Card> result = new List<Card>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(view_cards_by_userID, conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(ConvertFields(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        //List all cards in a deck in order
        public List<Card> ViewCardsInDeck(string deckID)
        {
            List<Card> result = new List<Card>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(view_cards_in_deck, conn);

                    cmd.Parameters.AddWithValue("@deck_id", deckID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(ConvertFields(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        //get card object based on id
        public Card GetCardByID(string id)
        {
            Card currentCard = new Card();
            currentCard.CardID = id;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(get_card_by_cardID, conn);
                    cmd.Parameters.AddWithValue("@card_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        currentCard.UserID = Convert.ToString(reader["UserID"]);
                        currentCard.Front = Convert.ToString(reader["Front"]).Trim();
                        currentCard.Back = Convert.ToString(reader["Back"]).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return currentCard;
        }

        //create a new card
        public bool CreateCard(Card card, string user_id)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(create_card, conn);
                    cmd.Parameters.AddWithValue("@front", card.Front);
                    cmd.Parameters.AddWithValue("@back", card.Back);
                    cmd.Parameters.AddWithValue("@user_id", user_id);

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (result > 0);
        }

        //edit card front and back
        public bool EditCard(Card currentCard)
        {
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(edit_Card, conn);
                    cmd.Parameters.AddWithValue("@front", currentCard.Front);
                    cmd.Parameters.AddWithValue("@back", currentCard.Back);
                    cmd.Parameters.AddWithValue("@id", currentCard.CardID);

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (result > 0);
        }

        //search cards by tag
        public List<Card> SearchCard(string tagName)
        {
            List<Card> matchingCards = new List<Card>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(search_Card, conn);

                    cmd.Parameters.AddWithValue("@TagName", tagName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        matchingCards.Add(ConvertFields(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return matchingCards;
        }

        //Get user cards and Admin Cards
        public List<Card> ViewCardsWithAdminCards(string userID)
        {
            List<Card> result = new List<Card>();
            List<string> userIDList = this.PublicUserList();
            userIDList.Add(userID);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string userSplit = "";

                    foreach (string item in userIDList)
                    {
                        userSplit += item + ",";
                    }

                    SqlCommand cmd = new SqlCommand(view_cards_by_userID_with_Admin, conn);
                    cmd.Parameters.AddWithValue("@userSplitValue", userSplit);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(ConvertFields(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        //convert SQL to obj properties
        private Card ConvertFields(SqlDataReader reader)
        {
            Card card = new Card();
            card.CardID = Convert.ToString(reader["CardID"]);
            card.Front = Convert.ToString(reader["Front"]);
            card.Back = Convert.ToString(reader["Back"]);

            return card;
        }

        //Returns a list of userID's to associate with Public Cards
        private List<string> PublicUserList()
        {
            List<string> result = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getAdminListSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(Convert.ToString(reader["UserId"]).Trim());
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}