using AstroDAM.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AstroDAM.Controllers
{
    public static class PhotographerController
    {
        public static List<Photographer> GetPhotographers(List<int> Ids = null)
        {
            List<Photographer> photographers = new List<Photographer>();

            string selector = DbManager.SelectorBuilder(Ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[FirstName],[LastName] FROM [tblPhotographers] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);

                photographers.Add(new Photographer(id, firstName, lastName));
            }

            con.Close();
            return photographers;
        }

        public static void DeletePhotographers(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblPhotographers] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditPhotographer(int Id, Photographer photographer)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblPhotographers] SET " +
                "[FirstName] = @FirstName, " +
                "[LastName] = @LastName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LongName", photographer.LastName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static int AddPhotographer(Photographer photographer)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblPhotographers] ([FirstName],[LastName]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@FirstName,@LastName)";

            cmd.Parameters.AddWithValue("@FirstName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", photographer.LastName);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
