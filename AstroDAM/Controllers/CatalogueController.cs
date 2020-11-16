using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage catalogue records on the database.
    /// </summary>
    public static class CatalogueController
    {
        /// <summary>
        /// Get one or more catalogue records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the catalogue records.</param>
        /// <returns>A list of catalogue records.</returns>
        public static List<Catalogue> GetCatalogues(List<int> ids = null)
        {
            List<Catalogue> catalogues = new List<Catalogue>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName] FROM [tblCatalogues] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string shortName = reader.GetString(1);
                string longName = reader.GetString(2);

                catalogues.Add(new Catalogue(id, shortName, longName));
            }

            con.Close();
            return catalogues;
        }

        /// <summary>
        /// Delete one or more catalogue records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the catalogue records.</param>
        public static void DeleteCatalogues(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCatalogues] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a catalogue record on the database.
        /// </summary>
        /// <param name="catalogue">Catalogue records information.</param>
        /// 
        /// <remarks>Id property of the <paramref name="catalogue"/> parameter is ignored.</remarks>
        public static void EditCatalogue(Catalogue catalogue)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblCatalogues] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", catalogue.ShortName);
            cmd.Parameters.AddWithValue("@LongName", catalogue.LongName);
            cmd.Parameters.AddWithValue("@Id", "Id");

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a catalogue record to the database.
        /// </summary>
        /// <param name="catalogue">Catalogue record information.</param>
        /// <returns>ID of the new catalogue record.</returns>
        /// <remarks>Id property of the <paramref name="catalogue"/> parameter is ignored.</remarks>
        public static int AddCatalogue(Catalogue catalogue)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();
            int result;

            cmd.CommandText = "INSERT INTO [tblCatalogues] ([ShortName],[LongName]) " +
                "OUTPUT INSERTED.Id " +
                " VALUES(@ShortName,@LongName)";

            cmd.Parameters.AddWithValue("@ShortName", catalogue.ShortName);
            cmd.Parameters.AddWithValue("@LongName", catalogue.LongName);

            result = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();

            return result;
        }
    }
}
