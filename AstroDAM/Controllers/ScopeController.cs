using System.Collections.Generic;
using System.Data.SqlClient;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    /// <summary>
    /// Manage photographer records on the database.
    /// </summary>
    public static class ScopeController
    {
        /// <summary>
        /// Get one or more scope records on the database.
        /// </summary>
        /// <param name="ids">List of IDs of the scope records.</param>
        /// <returns>A list of scope records.</returns>
        public static List<Scope> GetScopes(List<int> ids = null)
        {
            List<Scope> scopes = new List<Scope>();

            string selector = DbManager.SelectorBuilder(ids);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Manufacturer],[Name],[Aperture],[FocalLength],[CentralObstructionDiameter],[Robotic],[MountType] FROM [tblScopes] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string manufacturer = reader.GetString(1);
                string name = reader.GetString(2);
                float aperture = (float)reader.GetDouble(3);
                float focalLength = (float)reader.GetDouble(4);
                float centralObstructionDiameter = (float)reader.GetDouble(5);
                bool robotic = reader.GetBoolean(6);
                int mountType = reader.GetByte(7);

                scopes.Add(new Scope(id, manufacturer, name, aperture, focalLength, centralObstructionDiameter, robotic, (Scope.MountTypes)mountType));
            }

            con.Close();
            return scopes;
        }

        /// <summary>
        /// Delete one or more scope records.
        /// </summary>
        /// <param name="ids">List of IDs of the scope records.</param>
        public static void DeleteScopes(List<int> ids = null)
        {
            string selector = DbManager.SelectorBuilder(ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblScopes] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Edit a scope record on the database.
        /// </summary>
        /// <param name="id">ID of the scope record to edit.</param>
        /// <param name="scope">Scope records information.</param>
        /// <remarks>Id property of the <paramref name="scope"/> parameter is ignored.</remarks>
        public static void EditScope(int id, Scope scope)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblScopes] SET " +
                "[Manufacturer] = @Manufacturer, " +
                "[Name] = @Name, " +
                "[Aperture] = @Aperture, " +
                "[FocalLength] = @FocalLength, " +
                "[CentralObstructionDiameter] = @CentralObstructionDiameter, " +
                "[Robotic] = @Robotic, " +
                "[MountType] = @MountType " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Manufacturer", scope.Manufacturer);
            cmd.Parameters.AddWithValue("@Name", scope.Name);
            cmd.Parameters.AddWithValue("@Aperture", scope.Aperture);
            cmd.Parameters.AddWithValue("@FocalLength", scope.FocalLength);
            cmd.Parameters.AddWithValue("@CentralObstructionDiameter", scope.CentralObstructionDiameter);
            cmd.Parameters.AddWithValue("@Robotic", scope.Robotic);
            cmd.Parameters.AddWithValue("@MountType", (int)scope.MountType);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Add a scope record to the database.
        /// </summary>
        /// <param name="scope">Scope record information.</param>
        /// <returns>ID of the new scope record.</returns>
        /// <remarks>Id property of the <paramref name="scope"/> parameter is ignored.</remarks>
        public static int AddScope(Scope scope)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblScopes] ([Manufacturer],[Name],[Aperture],[FocalLength],[CentralObstructionDiameter],[Robotic],[MountType]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Manufacturer,@Name,@Aperture,@FocalLength,@CentralObstructionDiameter,@Robotic,@MountType)";

            cmd.Parameters.AddWithValue("@Manufacturer", scope.Manufacturer);
            cmd.Parameters.AddWithValue("@Name", scope.Name);
            cmd.Parameters.AddWithValue("@Aperture", scope.Aperture);
            cmd.Parameters.AddWithValue("@FocalLength", scope.FocalLength);
            cmd.Parameters.AddWithValue("@CentralObstructionDiameter", scope.CentralObstructionDiameter);
            cmd.Parameters.AddWithValue("@Robotic", scope.Robotic);
            cmd.Parameters.AddWithValue("@MountType", (int)scope.MountType);

            int res = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();
            return res;
        }
    }
}
