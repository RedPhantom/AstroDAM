using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM
{
    public static class Operations
    {
        public static SqlConnection GetCon()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.ConnectionString);
            con.Open();

            return con;
        }

        public static bool TestDbConnection(string ConnectionString = "")
        {
            if (ConnectionString == "")
                ConnectionString = Properties.Settings.Default.ConnectionString;

            if (string.IsNullOrEmpty(ConnectionString))
                return false;

            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT 1";

                return cmd.ExecuteScalar().ToString() == "1";
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CameraId">Specify a parameter for a specific camera, otherwise returns all cameras.</param>
        /// <returns></returns>
        public static List<Camera> GetCameras(int CameraId = -1)
        {
            List<Camera> Cameras = new List<Camera>();

            SqlConnection con = GetCon();
            SqlCommand cmd = new SqlCommand();

            cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName],[MaxResolution],[ColorSpaces] FROM [tblCameras]";
            if (CameraId != -1)
            {
                cmd.CommandText += " WHERE [Id] = @Id";
                cmd.Parameters.AddWithValue("@Id", CameraId);
            }

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

            return Cameras;
        }

        public static void DeleteCameras(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);

            
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();
            
            cmd.CommandText = "DELETE FROM [tblCameras] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static void EditCamera(int Id, Camera camera)
        {

        }

        public static List<Catalogue> GetCatalogues(List<int> Ids = null)
        {
            List<Catalogue> catalogues = new List<Catalogue>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName] FROM [tblCatalogues] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string shortName = reader.GetString(1);
                string longName = reader.GetString(2);

                catalogues.Add(new Catalogue(id, shortName, longName));
            }

            return catalogues;
        }

        public static void DeleteCatalogues(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCatalogues] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<Collection> GetCollections(List<int> Ids = null)
        {
            List<Collection> Collections = new List<Collection>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[CaptureDateTime],[CatalogueId],[ObjectId],[ObjectTitle]," +
                "[NumberFrames],[FileFormat],[ColorSpace],[Resolution] FROM [tblCollections] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime captureDateTime = reader.GetDateTime(1);
                Catalogue catalogue = GetCatalogues(new List<int>() { reader.GetInt32(2) })[0];
                int objectId = reader.GetInt32(3);
                string objectTitle = reader.GetString(4);
                int numberFrames = reader.GetInt32(5);
                FileFormat fileFormat = GetFileFormats(new List<int>() { reader.GetInt32(6) })[0];
                ColorSpace colorSpace = GetColorSpaces(new List<int>() { reader.GetInt32(7) })[0];
                Size resolution = ParseResolution(reader.GetString(8));

                Collections.Add(new Collection(id, captureDateTime, catalogue, objectId, objectTitle, numberFrames, fileFormat, colorSpace, resolution));
            }

            return Collections;
        }

        public static void DeleteCollections(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCollections] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<ColorSpace> GetColorSpaces(List<int> Ids = null)
        {
            List<ColorSpace> ColorSpaces = new List<ColorSpace>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Name],[BytesPerChannel],[IsMultiChannel] FROM [tblColorSpaces] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                byte bitsPerChannel = reader.GetByte(2);
                bool isMultiChannel = reader.GetBoolean(3);

                ColorSpaces.Add(new ColorSpace(id, name, bitsPerChannel, isMultiChannel));
            }

            return ColorSpaces;
        }

        public static void DeleteColorSpaces(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblColorSpaces] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<FileFormat> GetFileFormats(List<int> Ids = null)
        {
            List<FileFormat> fileFormats = new List<FileFormat>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
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

            return fileFormats;
        }

        public static void DeleteFileFormats(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblFileFormats] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<Optic> GetOptics(List<int> Ids = null)
        {
            List<Optic> optics = new List<Optic>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Type],[Value] FROM [tblOptics] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int type = reader.GetInt32(1);
                float value = reader.GetFloat(2);

                optics.Add(new Optic(id, (Optic.OpticTypes)type, value));
            }

            return optics;
        }
        
        public static void DeleteOptics(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblOptics] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<Photographer> GetPhotographers(List<int> Ids = null)
        {
            List<Photographer> photographers = new List<Photographer>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[FirstName],[LastName] FROM [tblPhotographers] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);

                photographers.Add(new Photographer(id, firstName, lastName));
            }

            return photographers;
        }

        public static void DeletePhotographers(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblPhotographers] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<Scope> GetScopes(List<int> Ids = null)
        {
            List<Scope> scopes = new List<Scope>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Manufacturer],[Name],[Aperture],[FocalLength],[CentralObstructionDiameter],[Robotic],[MountType] FROM [tblScopes] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string manufacturer = reader.GetString(1);
                string name = reader.GetString(2);
                float aperture = reader.GetFloat(3);
                float focalLength = reader.GetFloat(4);
                float centralObstructionDiameter = reader.GetFloat(5);
                bool robotic = reader.GetBoolean(6);
                int mountType = reader.GetByte(7);

                scopes.Add(new Scope(id, manufacturer, name, aperture, focalLength, centralObstructionDiameter, robotic, (Scope.MountTypes)mountType));
            }

            return scopes;
        }

        public static void DeleteScopes(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblScopes] " + selector;

            cmd.ExecuteNonQuery();
        }

        public static List<Site> GetSites(List<int> Ids = null)
        {
            List<Site> sites = new List<Site>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType], FROM [tblSites] " + selector;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                float longtitude = reader.GetFloat(2);
                bool longtitudeType = reader.GetBoolean(3);
                float latitude = reader.GetFloat(4);
                bool latitudeType = reader.GetBoolean(5);

                sites.Add(new Site(id, name, longtitude, longtitudeType ? Site.LongtitudeTypes.North : Site.LongtitudeTypes.South,
                    latitude, latitudeType ? Site.LatitudeTypes.East : Site.LatitudeTypes.West));
            }

            return sites;
        }

        public static void DeleteSites(List<int> Ids = null)
        {
            string selector = SelectorBuilder(Ids, false);


            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblSites] " + selector;

            cmd.ExecuteNonQuery();
        }

        #region Utility Functions
        public static Size ParseResolution(string str)
        {
            List<int> xyData = ParseIntList(str);

            return new Size(xyData[0], xyData[1]);
        }

        public static List<int> ParseIntList(string str)
        {
            string[] nums = str.Split(';');
            List<int> numList = new List<int>(nums.Length);

            foreach (string num in nums)
                numList.Add(int.Parse(num));

            return numList;
        }

        public static string SelectorBuilder(List<int> Ids, bool canBeEmpty = true)
        {
            string selector = "";

            if (Ids != null)
            {
                if (Ids.Count > 0)
                    selector += "WHERE [Id]=" + Ids[0];

                if (Ids.Count > 1)
                {
                    for (int i = 1; i < Ids.Count; i++)
                    {
                        selector += " OR [Id]=" + Ids[i].ToString();

                    }
                }
            }

            if (selector == "" && !canBeEmpty)
                throw new ArgumentException("Selector can't be empty.");

            return selector;
        }
        #endregion
    }
}
