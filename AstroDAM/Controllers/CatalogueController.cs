using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Controllers
{
    public static class CatalogueController
    {
        public static List<Catalogue> GetCatalogues(List<int> Ids = null)
        {
            List<Catalogue> catalogues = new List<Catalogue>();

            string selector = DbManager.SelectorBuilder(Ids);

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

        public static void DeleteCatalogues(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCatalogues] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditCatalogue(int Id, Catalogue catalogue)
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

        public static int AddCatalogue(Catalogue catalogue)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();
            int result = -1;

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
