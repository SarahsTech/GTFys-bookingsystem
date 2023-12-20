using GTFys.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFysUnitTest
{
    [TestClass]
    public class DatabaseConnectionTest
    {
        [TestMethod]
        public void TestDatabaseConnection()
        {
            // Arrange
            DatabaseConnection databaseConnection = new DatabaseConnection();

            // Act
            IDbConnection connection = databaseConnection.Connect();

            // Assert
            // Check if the connection is not null, indicating a successful connection
            Assert.IsNotNull(connection, "Database connection is null");

            // Check if the connection is open
            Assert.IsTrue(connection.State == ConnectionState.Open, "Database connection is not open");

            // Check if the connection string is accessible
            Assert.IsFalse(string.IsNullOrEmpty(databaseConnection.ConnectionString), "Connection string is null or empty");

            // Cleanup
            // Disconnect from the database
            databaseConnection.Disconnect(connection);
        }
    }
}
