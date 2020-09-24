using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM
{
    public static class DbManager
    {
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
                        Application.Exit();
                }
            }

            throw new Exception("Repeating database exception.");
        }

        public static bool TestConnection(string ConnectionString = "")
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

                bool res = cmd.ExecuteScalar().ToString() == "1";
                con.Close();
                return res;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string SelectorBuilder(List<int> Ids, bool canBeEmpty = true)
        {
            string selector = "";

            if (Ids != null)
            {
                if (Ids.Count > 0)
                    selector += "WHERE [Id]=" + Ids[0];

                if (Ids.Count > 1)
                {
                    for (int i = 1; i < Ids.Count; i++)
                    {
                        selector += " OR [Id]=" + Ids[i].ToString();

                    }
                }
            }

            if (selector == "" && !canBeEmpty)
                throw new ArgumentException("Selector can't be empty.");

            return selector;
        }
    }
}
