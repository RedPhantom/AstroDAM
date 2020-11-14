using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    public static class FileFormatController
    {
        public static List<FileFormat> GetFileFormats(List<int> Ids = null)
        {
            List<FileFormat> fileFormats = new List<FileFormat>();

            string selector = DbManager.SelectorBuilder(Ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName] FROM [tblFileFormats] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string shortName = reader.GetString(1);
                string longName = reader.GetString(2);

                fileFormats.Add(new FileFormat(id, shortName, longName));
            }

            con.Close();
            return fileFormats;
        }

        public static void DeleteFileFormats(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblFileFormats] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditFileFormat(int Id, FileFormat fileFormat)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblFileFormats] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static int AddFileFormat(FileFormat fileFormat)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblFileFormats] ([ShortName],[LongName]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@ShortName,@LongName)";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
