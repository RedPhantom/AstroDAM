using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    public static class ColorSpaceController
    {
        public static List<ColorSpace> GetColorSpaces(List<int> Ids = null)
        {
            List<ColorSpace> ColorSpaces = new List<ColorSpace>();

            string selector = DbManager.SelectorBuilder(Ids);

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

        public static void DeleteColorSpaces(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblColorSpaces] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditColorSpace(int Id, ColorSpace colorSpace)
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
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

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
