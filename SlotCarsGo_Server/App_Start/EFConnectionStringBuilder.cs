using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Web;

namespace SlotCarsGo_Server.App_Start
{
    public class EFConnectionStringBuilder
    {
        // Specify the provider name, server and database.
        private static string providerName = "System.Data.SqlClient";
        //        private static string serverName = "db725800987.db.1and1.com";
        //        private static string databaseName = "db725800987";
        private static string serverName = @"(localdb)\MSSQLLocalDB";
        private static string databaseName = "slotcarsgo_dev";
        private static string contextClass = "SlotCarsGo_Server.Models.ApplicationDbContext";
            

        public  string BuildEFConnectionString()
        {
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            
            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;


            // Set the Metadata location.
            entityBuilder.Metadata = $@"res://*/{contextClass}.csdl| res://*/{contextClass}.ssdl| res://*/{contextClass}.msl";
            Console.WriteLine(entityBuilder.ToString());

            using (EntityConnection conn =
                new EntityConnection(entityBuilder.ToString()))
            {
                conn.Open();
                Console.WriteLine("Just testing the connection.");
                conn.Close();
            }

            return entityBuilder.ToString();
        }
    }
}