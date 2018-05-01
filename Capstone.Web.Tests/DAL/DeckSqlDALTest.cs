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
    public class DeckSqlDALTest
    {
        private TransactionScope tran;

        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        private int numDecks = 0;
        private int deckID = 0;
        private int cardID = 0;

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

                cmd = new SqlCommand(@"SELECT COUNT(*) FROM decks", conn);
                numDecks = (int)cmd.ExecuteScalar();

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
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
            //tran.Complete();
        }

        [TestMethod]
        public void GetDecksTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            List<Deck> deckList = deckSql.GetDecksByUserID("2");

            //Assert
            Assert.AreEqual(numDecks + 1, deckList.Count);
        }

        [TestMethod]
        public void GetDecksByNameTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            List<Deck> deckList = deckSql.SearchDecksByName("2", "SQL Test");

            //Assert
            Assert.AreEqual(1, deckList.Count);
        }

        [TestMethod]
        public void AddDeckTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            int newDeckID = int.Parse(deckSql.AddDeck("2", "SQL Test Add"));

            //Assert
            Assert.AreEqual(deckID + 1, newDeckID);
        }

        [TestMethod]
        public void ModifyDeckNameTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            bool success = deckSql.ModifyDeckName(deckID.ToString(), "SQL Test Changed");
            List<Deck> deckList = deckSql.SearchDecksByName("2", "SQL Test Changed");

            //Assert
            Assert.IsTrue(success);
            Assert.AreEqual(1, deckList.Count);
        }

        [TestMethod]
        public void ModifyDeckPublicTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            bool success = deckSql.ModifyDeckIsPublic(deckID.ToString(), true);
            Deck deckTest = deckSql.GetDeckByDeckID(deckID.ToString());

            //Assert
            Assert.IsTrue(success);
            Assert.AreEqual(true, deckTest.IsPublic);
        }

        [TestMethod]
        public void AddCardToDeckTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);

            //Act
            bool success = deckSql.AddCardToDeck(cardID.ToString(), deckID.ToString());

            //Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void RemoveCardFromDeckTest()
        {
            //Arrange
            DeckSqlDAL deckSql = new DeckSqlDAL(connectionString);
            Deck testDeck = deckSql.GetDeckByDeckID(deckID.ToString());

            //Act
            testDeck.AddCardToDeck(cardID.ToString());
            bool deleted = deckSql.RemoveCardFromDeck(cardID.ToString(), deckID.ToString());

            //Assert
            Assert.IsTrue(deleted);
        }
    }
}
