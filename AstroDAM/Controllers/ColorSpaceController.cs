using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage color space records on the database.
    /// </summary>
    public static class ColorSpaceController
    {
        /// <summary>
        /// Get one or more color space records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the color space records.</param>
        /// <returns>A list of color space records.</returns>
        public static List<ColorSpace> GetColorSpaces(List<int> ids = null)
        {
            List<ColorSpace> ColorSpaces = new List<ColorSpace>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Name],[BitsPerChannel],[IsMultiChannel] FROM [tblColorSpaces] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                byte bitsPerChannel = reader.GetByte(2);
                bool isMultiChannel = reader.GetBoolean(3);

                ColorSpaces.Add(new ColorSpace(id, name, bitsPerChannel, isMultiChannel));
            }

            con.Close();
            return ColorSpaces;
        }

        /// <summary>
        /// Delete one or more color space records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the color space records.</param>
        public static void DeleteColorSpaces(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblColorSpaces] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a color space record on the database.
        /// </summary>
        /// <param name="id">ID of the color space record to edit.</param>
        /// <param name="colorSpace">Catalogue records information.</param>
        /// <remarks>Id property of the <paramref name="colorSpace"/> parameter is ignored.</remarks>
        public static void EditColorSpace(int id, ColorSpace colorSpace)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblColorSpaces] SET " +
                "[Name] = @Name, " +
                "[BitsPerChannel] = @BitsPerChannel, " +
                "[IsMultiChannel] = @IsMultiChannel, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Name", colorSpace.Name);
            cmd.Parameters.AddWithValue("@BitsPerChannel", colorSpace.BitsPerChannel);
            cmd.Parameters.AddWithValue("@IsMultiChannel", colorSpace.IsMultiChannel);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a color space record to the database.
        /// </summary>
        /// <param name="colorSpace">Color space record information.</param>
        /// <returns>ID of the new color space record.</returns>
        /// <remarks>Id property of the <paramref name="colorSpace"/> parameter is ignored.</remarks>
        public static int AddColorSpace(ColorSpace colorSpace)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblColorSpaces] ([Name],[BitsPerChannel],[IsMultiChannel]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name,@BitsPerChannel,@IsMultiChannel)";

            cmd.Parameters.AddWithValue("@Name", colorSpace.Name);
            cmd.Parameters.AddWithValue("@BitsPerChannel", colorSpace.BitsPerChannel);
            cmd.Parameters.AddWithValue("@IsMultiChannel", colorSpace.IsMultiChannel);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
