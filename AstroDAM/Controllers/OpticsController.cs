using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage optics records on the database.
    /// </summary>
    public static class OpticsController
    {
        /// <summary>
        /// Get one or more optics records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the optics records.</param>
        /// <returns>A list of optics records.</returns>
        public static List<Optic> GetOptics(List<int> ids = null)
        {
            List<Optic> optics = new List<Optic>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Type],[Value] FROM [tblOptics] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int type = reader.GetInt32(1);
                float value = (float)(float)reader.GetDouble(2);

                optics.Add(new Optic(id, (Optic.OpticTypes)type, value));
            }

            con.Close();
            return optics;
        }

        /// <summary>
        /// Delete one or more optics records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the optics records.</param>
        public static void DeleteOptics(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblOptics] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit an optics record on the database.
        /// </summary>
        /// <param name="id">ID of the optics record to edit.</param>
        /// <param name="optic">Optics records information.</param>
        /// <remarks>Id property of the <paramref name="optic"/> parameter is ignored.</remarks>
        public static void EditOptics(int id, Optic optic)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblOptics] SET " +
                "[Type] = @Type, " +
                "[Value] = @Value " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add an optics record to the database.
        /// </summary>
        /// <param name="optic">Optics record information.</param>
        /// <returns>ID of the new optics record.</returns>
        /// <remarks>Id property of the <paramref name="optic"/> parameter is ignored.</remarks>
        public static int AddOptics(Optic optic)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblOptics] ([Type],[Value]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Type,@Value)";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
