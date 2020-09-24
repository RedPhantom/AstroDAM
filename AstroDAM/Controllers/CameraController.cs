using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Controllers
{
    public static class CameraController
    {
        public static List<Camera> GetCameras(List<int> Ids = null)
        {
            List<Camera> Cameras = new List<Camera>();

            string selector = DbManager.SelectorBuilder(Ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = new SqlCommand();

            cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName],[MaxResolution],[ColorSpaces] FROM [tblCameras] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string shortName = reader.GetString(1);
                string longName = reader.GetString(2);
                Size maxResolution = ParseResolution(reader.GetString(3));
                List<ColorSpace> colorSpaces = GetColorSpaces(ParseIntList(reader.GetString(4)));

                Cameras.Add(new Camera(id, shortName, longName, maxResolution, colorSpaces));
            }

            con.Close();
            return Cameras;
        }

        public static void DeleteCameras(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCameras] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditCamera(int Id, Camera camera)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblCameras] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName, " +
                "[MaxResolution] = @MaxResolution, " +
                "[ColorSpaces] = @ColorSpaces " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", camera.ShortName);
            cmd.Parameters.AddWithValue("@LongName", camera.LongName);
            cmd.Parameters.AddWithValue("@MaxResolution", MakeResolution(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", MakeIntList(camera.ColorSpaces.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static int AddCamera(Camera camera)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCameras] ([ShortName],[LongName],[MaxResolution],[ColorSpaces]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@ShortName,@LongName,@MaxResolution,@ColorSpaces)";

            cmd.Parameters.AddWithValue("@ShortName", camera.ShortName);
            cmd.Parameters.AddWithValue("@LongName", camera.LongName);
            cmd.Parameters.AddWithValue("@MaxResolution", MakeResolution(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", MakeIntList(camera.ColorSpaces.Select(x => x.Id).ToList()));

            con.Close();
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
