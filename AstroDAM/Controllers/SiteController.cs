using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage site records on the database.
    /// </summary>
    public static class SiteController
    {
        /// <summary>
        /// Get one or more site records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the site records.</param>
        /// <returns>A list of site records.</returns>
        public static List<Site> GetSites(List<int> ids = null)
        {
            List<Site> sites = new List<Site>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType] FROM [tblSites] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                float longtitude = (float)reader.GetDouble(2);
                bool longtitudeType = reader.GetBoolean(3);
                float latitude = (float)reader.GetDouble(4);
                bool latitudeType = reader.GetBoolean(5);

                sites.Add(new Site(id, name, longtitude, longtitudeType ? Site.LongtitudeTypes.North : Site.LongtitudeTypes.South,
                    latitude, latitudeType ? Site.LatitudeTypes.East : Site.LatitudeTypes.West));
            }

            con.Close();
            return sites;
        }

        /// <summary>
        /// Delete one or more site records.
        /// </summary>
        /// <param name="ids">List of IDs of the site records.</param>
        public static void DeleteSites(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblSites] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a site record on the database.
        /// </summary>
        /// <param name="ids">ID of the site record to edit.</param>
        /// <param name="site">Site records information.</param>
        /// <remarks>Id property of the <paramref name="site"/> parameter is ignored.</remarks>
        public static void EditSite(int ids, Site site)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblSites] SET " +
                "[Name] = @Name, " +
                "[Longtitude] = @Longtitude, " +
                "[LongtitudeType] = @LongtitudeType, " +
                "[Latitude] = @Latitude, " +
                "[LatitudeType] = @LatitudeType " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);
            cmd.Parameters.AddWithValue("@Id", ids);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a site record to the database.
        /// </summary>
        /// <param name="site">Site record information.</param>
        /// <returns>ID of the new site record.</returns>
        /// <remarks>Id property of the <paramref name="site"/> parameter is ignored.</remarks>
        public static int AddSite(Site site)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblSites] ([Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name,@Longtitude,@LongtitudeType,@Latitude,@LatitudeType)";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
