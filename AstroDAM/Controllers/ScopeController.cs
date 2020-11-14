using System.Collections.Generic;
using System.Data.SqlClient;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    public static class ScopeController
    {
        public static List<Scope> GetScopes(List<int> Ids = null)
        {
            List<Scope> scopes = new List<Scope>();

            string selector = DbManager.SelectorBuilder(Ids);

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

            return scopes;
            con.Close();
        }

        public static void DeleteScopes(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblScopes] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditScope(int Id, Scope scope)
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
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

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
