using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Controllers
{
    public static class SiteController
    {
        public static List<Site> GetSites(List<int> Ids = null)
        {
            List<Site> sites = new List<Site>();

            string selector = DbManager.SelectorBuilder(Ids);

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

        public static void DeleteSites(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblSites] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditSite(int Id, Site site)
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
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

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
