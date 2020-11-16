using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AstroDAM
{
    /// <summary>
    /// Manage database connection and queries.
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// Retrieve a database connection.
        /// </summary>
        /// <returns>An <see cref="SqlConnection"/> object, tested and ready to connect.</returns>
        public static SqlConnection GetConnection()
        {
            for (int i = 1; i < 6; i++)
            {
                try
                {
                    SqlConnection con = new SqlConnection(Properties.Settings.Default.ConnectionString);
                    con.Open();

                    return con;
                }
                catch (Exception ex)
                {
                    DialogResult dr = MessageBox.Show(string.Format("An error occured while approaching the server:\n{0}\nTrying again ({1}/5). Quit?", ex.Message, i), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (dr == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }
            }

            throw new Exception("Exceeded number of connection retries.");
        }

        /// <summary>
        /// Test the database connection.
        /// </summary>
        /// <param name="ConnectionString">Connection string to use when connecting to the database.</param>
        /// <returns>True if the connection was successful.</returns>
        public static bool TestConnection(string ConnectionString = "")
        {
            if (ConnectionString == "")
            {
                ConnectionString = Properties.Settings.Default.ConnectionString;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                return false;
            }

            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT 1";

                bool res = cmd.ExecuteScalar().ToString() == "1";
                con.Close();
                return res;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Build a selector (WHERE SQL cluase).
        /// </summary>
        /// <param name="ids">List of record IDs to select.</param>
        /// <param name="canBeEmpty">Whether the list can be empty.</param>
        /// <returns>An SQL WHERE clause.</returns>
        public static string SelectorBuilder(List<int> ids, bool canBeEmpty = true)
        {
            string selector = "";

            if (ids != null)
            {
                if (ids.Count > 0) 
                { 
                    selector += "WHERE [Id]=" + ids[0];
                }

                if (ids.Count > 1)
                {
                    for (int i = 1; i < ids.Count; i++)
                    {
                        selector += " OR [Id]=" + ids[i].ToString();
                    }
                }
            }

            if (selector == "" && !canBeEmpty)
            {
                throw new ArgumentException("Selector can't be empty.");
            }

            return selector;
        }
    }
}
