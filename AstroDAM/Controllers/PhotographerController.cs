using AstroDAM.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage photographer records on the database.
    /// </summary>
    public static class PhotographerController
    {
        /// <summary>
        /// Get one or more photograpger records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the photograpger records.</param>
        /// <returns>A list of photographer records.</returns>
        public static List<Photographer> GetPhotographers(List<int> ids = null)
        {
            List<Photographer> photographers = new List<Photographer>();

            string selector = DbManager.SelectorBuilder(ids);

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

        /// <summary>
        /// Delete one or more photographer records.
        /// </summary>
        /// <param name="ids">List of IDs of the photographer records.</param>
        public static void DeletePhotographers(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblPhotographers] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a photographer record on the database.
        /// </summary>
        /// <param name="id">ID of the photographer record to edit.</param>
        /// <param name="photographer">Photographer records information.</param>
        /// <remarks>Id property of the <paramref name="photographer"/> parameter is ignored.</remarks>
        public static void EditPhotographer(int id, Photographer photographer)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblPhotographers] SET " +
                "[FirstName] = @FirstName, " +
                "[LastName] = @LastName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LongName", photographer.LastName);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a photographer record to the database.
        /// </summary>
        /// <param name="photographer">Photographer record information.</param>
        /// <returns>ID of the new photographer record.</returns>
        /// <remarks>Id property of the <paramref name="photographer"/> parameter is ignored.</remarks>
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
