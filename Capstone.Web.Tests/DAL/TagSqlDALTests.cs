using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using Capstone.Web.DAL;
using Capstone.Web.Models;
using System.Collections.Generic;

namespace Capstone.Web.Tests.DAL
{
    [TestClass]
    public class TagSqlDALTests
    {
        [TestClass]
        public class DeckSqlDALTest
        {
            private TransactionScope tran;

            private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
            private int numTags = 0;
            private int deckID = 0;
            private int cardID = 0;
            private int tagID = 0;

            [TestInitialize]
            public void TestInitialize()
            {
                // Initialize a new transaction scope. This automatically begins the transaction.
                tran = new TransactionScope();

                // Open a SqlConnection object using the active transaction
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;

                    conn.Open();

                    cmd = new SqlCommand(@"SELECT COUNT(*) FROM tags", conn);
                    numTags = (int)cmd.ExecuteScalar();

                    //Insert a Dummy Record for Deck
                    // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                    // This will get the newest identity value generated for the record most recently inserted
                    cmd = new SqlCommand(@"INSERT INTO decks ([UserID], [Name], [IsPublic]) VALUES ('2', 'SQL Test', '1');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    deckID = (int)cmd.ExecuteScalar();

                    //Insert a Dummy Record for Card
                    // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                    // This will get the newest identity value generated for the record most recently inserted
                    cmd = new SqlCommand(@"INSERT INTO cards ([UserID], [Front], [Back]) VALUES ('2', 'SQL Test', '1');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    cardID = (int)cmd.ExecuteScalar();

                    //Insert a Dummy Record for Tags
                    // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                    // This will get the newest identity value generated for the record most recently inserted
                    cmd = new SqlCommand(@"INSERT INTO tags ([TagName]) VALUES ('SQL Test');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                    tagID = (int)cmd.ExecuteScalar();
                    

                }
            }

            [TestCleanup]
            public void Cleanup()
            {
                tran.Dispose();
                //tran.Complete();
            }

            [TestMethod]
            public void TagListTest()
            {
                //Arrange
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);

                //Act
                List<string> tagList = tagSql.TagList;

                //Assert
                Assert.AreEqual(numTags + 1, tagList.Count);
            }

            [TestMethod]
            public void AddTagTest()
            {
                //Arrange
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);

                //Act
                int newTagID = int.Parse(tagSql.AddTag("SQL Test Add"));
                int existingTagID = int.Parse(tagSql.AddTag("SQL Test Add"));

                //Assert
                Assert.AreEqual(tagID + 1, newTagID);
                Assert.AreEqual(newTagID, existingTagID);
            }

            [TestMethod]
            public void AddTagToCardTest()
            {
                //Arrange
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);

                //Act
                bool success = tagSql.AddTagToCard(cardID.ToString(), tagID.ToString());
                List<string> tagList = tagSql.GetTagsByCardID(cardID.ToString());

                //Assert
                Assert.IsTrue(success);
                Assert.AreEqual(1, tagList.Count);
            }

            [TestMethod]
            public void AddTagToDeckTest()
            {
                //Arrange
                TagsSqlDAL tagSql = new TagsSqlDAL(connectionString);

                //Act
                bool success = tagSql.AddTagToDeck(deckID.ToString(), tagID.ToString());
                List<string> tagList = tagSql.GetTagsByDeckID(deckID.ToString());

                //Assert
                Assert.IsTrue(success);
                Assert.AreEqual(1, tagList.Count);
            }
        }
    }
}
