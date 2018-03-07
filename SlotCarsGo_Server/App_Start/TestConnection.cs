using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Web;

namespace SlotCarsGo_Server.App_Start
{
    public class TestConnection
    {
        // Specify the provider name, server and database.
        private static string providerName = "System.Data.SqlClient";
        private static string serverName = "db725800987.db.1and1.com";
        private static string databaseName = "db725800987";
//        private static string serverName = @"(localdb)\MSSQLLocalDB";
//        private static string databaseName = "slotcarsgo_dev";
        private static string contextClass = "SlotCarsGo_Server.Models.ApplicationDbContext";

        public static string ConnectToDBFromWebConfig()
        {
            string message = "Running...";

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    message = "Connection opened.";
                    conn.Dispose();

                }
                catch (Exception e)
                {
                    message = $"FAILED: { e.Message}\n{e.StackTrace}\n{e.Source}, Connection String: {connStr}";
                }

            }



            return message;
        }

        public static string EntityDB()
        {
            ApplicationDbContext db;
            string message = "running";
            try
            {
                using (db = new ApplicationDbContext())
                {
                    message += $"DbContext: { db.Database.ToString()} ";
                    Driver driver = db.Drivers.FindAsync("11").Result;
                    message += "Called EF and not crashed.";
                    if (driver != null)
                    {
                        message += "Found!";
                    }
                }
            }
            catch (Exception e)
            {
                message = $"FAILED: { e.Message}\n{e.StackTrace}\n{e.Source}";
            }

            return message;
        }


        public static string ConnectToDBStaticCreds()
        {
            string message = "Running...";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;

            // Build the SqlConnection connection string.
            string connectionString = sqlBuilder.ToString();

            string userId = "dbo725800987";
            string password = "Sl0tC4rsG0!";

            using (SecureString s_password = new SecureString())
            {
                foreach (char c in password)
                {
                    s_password.AppendChar(c);
                }

                SqlCredential sqlCredential = new SqlCredential(userId, s_password);


                using (SqlConnection conn = new SqlConnection(connectionString, sqlCredential))
                {
                    try
                    {
                        conn.Open();
                        message = "Connection opened.";
                        conn.Dispose();
                    }
                    catch (Exception e)
                    {
                        message = $"FAILED: { e.Message}\n{e.StackTrace}\n{e.Source}";
                    }

                }
            }

            return message;
        }
    }
}