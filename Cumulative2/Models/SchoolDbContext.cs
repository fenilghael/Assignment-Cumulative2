using MySql.Data.MySqlClient;

namespace Cumulative2.Models
{
    /// <summary>
    /// Represents the database context for accessing the education database.
    /// </summary>
    public class EducationDbContext
    {
        // Credentials for accessing the database
        private static string DbUser { get { return "root"; } }
        private static string DbPassword { get { return "root"; } }
        private static string DbName { get { return "education"; } }
        private static string DbServer { get { return "localhost"; } }
        private static string DbPort { get { return "3306"; } }

        // Connection string used to establish a connection to the database
        protected static string DbConnectionString
        {
            get
            {
                return "server=" + DbServer
                    + ";user=" + DbUser
                    + ";database=" + DbName
                    + ";port=" + DbPort
                    + ";password=" + DbPassword
                    + ";convert zero datetime=True";
            }
        }

        /// <summary>
        /// Returns a connection to the education database.
        /// </summary>
        /// <returns>A MySqlConnection object representing the connection to the database.</returns>
        public MySqlConnection GetDatabaseConnection()
        {
            return new MySqlConnection(DbConnectionString);
        }
    }
}
