using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM
{
    public static class Operations
    {
        public static SqlConnection GetCon()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.ConnectionString);
            con.Open();

            return con;
        }
        public static bool TestDbConnection(string ConnectionString = "")
        {
            if (ConnectionString == "")
                ConnectionString = Properties.Settings.Default.ConnectionString;

            if (string.IsNullOrEmpty(ConnectionString))
                return false;

            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT 1";

                return cmd.ExecuteScalar().ToString() == "1";
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
