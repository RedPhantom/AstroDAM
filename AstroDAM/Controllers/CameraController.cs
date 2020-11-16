using System.Linq;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage camera records on the database.
    /// </summary>
    public static class CameraController
    {
        /// <summary>
        /// Get one or more camera records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the camera records.</param>
        /// <returns>A list of camera records.</returns>
        public static List<Camera> GetCameras(List<int> ids = null)
        {
            List<Camera> Cameras = new List<Camera>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName],[MaxResolution],[ColorSpaces] FROM [tblCameras] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string shortName = reader.GetString(1);
                string longName = reader.GetString(2);
                Size maxResolution = Utilities.StringToResolution(reader.GetString(3));
                List<ColorSpace> colorSpaces = ColorSpaceController.GetColorSpaces(
                    Utilities.StringToIntList(reader.GetString(4))
                );

                Cameras.Add(new Camera(id, shortName, longName, maxResolution, colorSpaces));
            }

            con.Close();
            return Cameras;
        }

        /// <summary>
        /// Delete one or more camera records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the camera records.</param>
        public static void DeleteCameras(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCameras] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a camera record on the database.
        /// </summary>
        /// <param name="id">ID of the camera record to edit.</param>
        /// <param name="camera">Camera record information.</param>
        /// <remarks>Id property of the <paramref name="camera"/> parameter is ignored.</remarks>
        public static void EditCamera(int id, Camera camera)
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
            cmd.Parameters.AddWithValue("@MaxResolution", Utilities.ResolutionToString(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", Utilities.IntListToString(camera.ColorSpaces.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a camera record to the database.
        /// </summary>
        /// <param name="camera">Camera record information.</param>
        /// <returns>ID of the new camera record.</returns>
        /// <remarks>Id property of the <paramref name="camera"/> parameter is ignored.</remarks>
        public static int AddCamera(Camera camera)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCameras] ([ShortName],[LongName],[MaxResolution],[ColorSpaces]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@ShortName,@LongName,@MaxResolution,@ColorSpaces)";

            cmd.Parameters.AddWithValue("@ShortName", camera.ShortName);
            cmd.Parameters.AddWithValue("@LongName", camera.LongName);
            cmd.Parameters.AddWithValue("@MaxResolution", Utilities.ResolutionToString(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", Utilities.IntListToString(camera.ColorSpaces.Select(x => x.Id).ToList()));

            con.Close();
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
