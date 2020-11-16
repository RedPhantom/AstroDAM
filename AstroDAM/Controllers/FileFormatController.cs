using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage file format records on the database.
    /// </summary>
    public static class FileFormatController
    {
        /// <summary>
        /// Get one or more file format records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the file format records.</param>
        /// <returns>A list of file format records.</returns>
        public static List<FileFormat> GetFileFormats(List<int> ids = null)
        {
            List<FileFormat> fileFormats = new List<FileFormat>();

            string selector = DbManager.SelectorBuilder(ids);

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

        /// <summary>
        /// Delete one or more file format records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the file format records.</param>
        public static void DeleteFileFormats(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblFileFormats] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a file format record on the database.
        /// </summary>
        /// <param name="id">ID of the file format record to edit.</param>
        /// <param name="fileFormat">File format records information.</param>
        /// <remarks>Id property of the <paramref name="fileFormat"/> parameter is ignored.</remarks>
        public static void EditFileFormat(int id, FileFormat fileFormat)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblFileFormats] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a file format record to the database.
        /// </summary>
        /// <param name="fileFormat">File format record information.</param>
        /// <returns>ID of the new file format record.</returns>
        /// <remarks>Id property of the <paramref name="fileFormat"/> parameter is ignored.</remarks>
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
