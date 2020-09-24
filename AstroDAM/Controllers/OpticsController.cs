using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Controllers
{
    public static class OpticsController
    {
        public static List<Optic> GetOptics(List<int> Ids = null)
        {
            List<Optic> optics = new List<Optic>();

            string selector = DbManager.SelectorBuilder(Ids);

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

        public static void DeleteOptics(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblOptics] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditOptics(int Id, Optic optic)
        {
            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblOptics] SET " +
                "[Type] = @Type, " +
                "[Value] = @Value " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

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
