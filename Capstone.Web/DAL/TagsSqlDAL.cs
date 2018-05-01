using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;
using System.Linq;
using System.Web;


namespace Capstone.Web.DAL
{
    public class TagsSqlDAL
    {
        private string connectionString;

        private string GetAllTagsSQL = "SELECT * FROM tags ORDER BY TagName";

        private string AddTagSQL = "INSERT INTO tags (TagName) VALUES (@tagNameValue);SELECT CAST(SCOPE_IDENTITY() as int);";

        private string AddTagToCardSQL = "INSERT INTO card_tag (CardID, TagID) VALUES (@cardIDValue, @tagIDValue);";
        private string GetTagsByCardIDSQL = "SELECT TagName FROM tags JOIN card_tag ON tags.TagID = card_tag.TagID WHERE card_tag.CardID = @cardIDValue ORDER BY TagName;";
        private string RemoveTagFromCardSQL = "DELETE FROM card_tag WHERE TagID = @tagIDValue AND CardID = @cardIDValue;";

        private string AddTagToDeckSQL = "INSERT INTO deck_tag (DeckID, TagID) VALUES (@deckIDValue, @tagIDValue);";
        private string GetTagsByDeckIDSQL = "SELECT TagName FROM tags JOIN deck_tag ON tags.TagID = deck_tag.TagID WHERE deck_tag.DeckID = @deckIDValue ORDER BY TagName;";
        private string RemoveTagFromDeckSQL = "DELETE FROM deck_tag WHERE TagID = @tagIDValue AND DeckID = @deckIDValue;";

        private string AdminDeleteTagSQL = "DELETE FROM tags WHERE TagID = @tagIDValue;";

        public TagsSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Dictionary<string, string> TagDictionary
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(GetAllTagsSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string resultKey = Convert.ToString(reader["TagName"]).Trim().ToLower();
                            string resultValue = Convert.ToString(reader["TagID"]);

                            result.Add(resultKey, resultValue);
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

        public List<SelectListItem> TagSelectItemList
        {
            get
            {
                List<SelectListItem> tagList = new List<SelectListItem>();

                foreach (KeyValuePair<string, string> item in this.TagDictionary)
                {
                    tagList.Add(new SelectListItem { Text = item.Key, Value = item.Value });
                }

                return tagList;
            }
        }

        public List<string> TagList
        {
            get
            {
                List<string> tagList = new List<string>();

                foreach (KeyValuePair<string, string> item in this.TagDictionary)
                {
                    tagList.Add(item.Key);
                }

                return tagList;
            }
        }

        /// <summary>
        /// Adds a new TagName to the database unless the TagName already exists.
        /// Returns the string TagID of the added TagName. If it already exists, it returns the existing TagID string.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string AddTag(string tagName)
        {
            if (!DoesTagExist(tagName))
            {
                string newTagID = "";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        var result = conn.ExecuteScalar<int>(AddTagSQL, new { tagNameValue = tagName });
                        if (result.ToString() != null)
                        {
                            newTagID = result.ToString();
                        }

                        return newTagID;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return this.TagDictionary[tagName.ToLower()];
            }
        }

        public bool DoesTagExist(string tagName)
        {
            if (this.TagDictionary.ContainsKey(tagName.Trim().ToLower()))
            {
                return true;
            }
            else return false;
        }

        public bool AddTagToCard(string cardID, string tagName)
        {
            bool success = false;
            if (!DoesTagExist(tagName))
            {
                string tagID = AddTag(tagName);
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        var result = conn.Execute(AddTagToCardSQL, new { cardIDValue = cardID, tagIDValue = tagID });
                        if (result == 1)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        var result = conn.Execute(AddTagToCardSQL, new { cardIDValue = cardID, tagIDValue = this.TagDictionary[tagName] });
                        if (result == 1)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return success;
        }

        public bool RemoveTagFromCard(string cardID, string tagName)
        {
            string removeTagID = this.TagDictionary[tagName.ToLower()];
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Execute(RemoveTagFromCardSQL, new { cardIDValue = cardID, tagIDValue = removeTagID });
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


        public List<string> GetTagsByCardID(string cardID)
        {
            List<string> result = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GetTagsByCardIDSQL, conn);
                    cmd.Parameters.AddWithValue("cardIDValue", cardID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string resultValue = Convert.ToString(reader["TagName"]).Trim();

                        result.Add(resultValue);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool AddTagToDeck(string deckID, string tagName)
        {
            bool success = false;
            if (!DoesTagExist(tagName))
            {
                string tagID = AddTag(tagName);
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        var result = conn.Execute(AddTagToDeckSQL, new { deckIDValue = deckID, tagIDValue = tagID });
                        if (result == 1)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        var result = conn.Execute(AddTagToDeckSQL, new { deckIDValue = deckID, tagIDValue = this.TagDictionary[tagName] });
                        if (result == 1)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return success;
        }

        public bool RemoveTagFromDeck(string deckID, string tagName)
        {
            string removeTagID = this.TagDictionary[tagName.ToLower()];
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Execute(RemoveTagFromDeckSQL, new { deckIDValue = deckID, tagIDValue = removeTagID });
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

        public List<string> GetTagsByDeckID(string deckID)
        {
            List<string> result = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(GetTagsByDeckIDSQL, conn);
                    cmd.Parameters.AddWithValue("deckIDValue", deckID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string resultValue = Convert.ToString(reader["TagName"]).Trim();

                        result.Add(resultValue);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool AdminDeleteTag(string tagName)
        {
            string removeTagID = this.TagDictionary[tagName.ToLower()];
            bool success = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var result = conn.Execute(AdminDeleteTagSQL, new { tagName = removeTagID });
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