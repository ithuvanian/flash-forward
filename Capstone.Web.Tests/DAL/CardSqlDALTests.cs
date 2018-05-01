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
    public class CardSqlDALTests
    {

        private TransactionScope tran;

        private string connectionString = ConfigurationManager.ConnectionStrings["HotelFlashCardsDB"].ConnectionString;
        private int numCards = 0;
        private int userID = -1;
        private int cardIDUser = 0;
        private int cardIDAdmin = 0;

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

                cmd = new SqlCommand(@"SELECT COUNT(*) FROM cards", conn);
                numCards = (int)cmd.ExecuteScalar();

                //Insert a Dummy Record for User as Admin
                // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                // This will get the newest identity value generated for the record most recently inserted
                cmd = new SqlCommand(@"INSERT INTO users ([Email], [Password], [IsAdmin], [UserName]) VALUES ('adminTest@hotel.com', 'SQL_Test', '1', 'TestAdmin');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                userID = (int)cmd.ExecuteScalar();

                //Insert a Dummy Record for Card for User
                // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                // This will get the newest identity value generated for the record most recently inserted
                cmd = new SqlCommand(@"INSERT INTO cards ([UserID], [Front], [Back]) VALUES ('2', 'SQL Test', '1');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                cardIDUser = (int)cmd.ExecuteScalar();

                //Insert a Dummy Record for Card for Admin
                // SELECT CAST(SCOPE_IDENTITY() as int) as a work-around
                // This will get the newest identity value generated for the record most recently inserted
                cmd = new SqlCommand(@"INSERT INTO cards ([UserID], [Front], [Back]) VALUES ('" + userID.ToString() + "', 'SQL Test', '1');SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                cardIDAdmin = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
            //tran.Complete();
        }

        [TestMethod]
        public void ViewAdminCardsTest()
        {
            //Arrange
            CardSqlDAL cardSql = new CardSqlDAL(connectionString);

            //Act
            List<Card> cardList = cardSql.ViewCardsWithAdminCards("2");

            //Assert
            Assert.AreEqual(numCards + 2, cardList.Count);
        }
    }
}
