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

        public static List<Camera> GetCameras(List<int> Ids = null)
        {
            List<Camera> Cameras = new List<Camera>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = new SqlCommand();

            cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[ShortName],[LongName],[MaxResolution],[ColorSpaces] FROM [tblCameras] " + selector;

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
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblCameras] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName, " +
                "[MaxResolution] = @MaxResolution, " +
                "[ColorSpaces] = @ColorSpaces " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", camera.ShortName);
            cmd.Parameters.AddWithValue("@LongName", camera.LongName);
            cmd.Parameters.AddWithValue("@MaxResolution", MakeResolution(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", MakeIntList(camera.ColorSpaces.Select(x => x.Id).ToList()));

            cmd.ExecuteNonQuery();
        }

        public static int AddCamera(Camera camera)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCameras] ([ShortName],[LongName],[MaxResolution],[ColorSpaces]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@ShortName,@LongName,@MaxResolution,@ColorSpaces)";

            cmd.Parameters.AddWithValue("@ShortName", camera.ShortName);
            cmd.Parameters.AddWithValue("@LongName", camera.LongName);
            cmd.Parameters.AddWithValue("@MaxResolution", MakeResolution(camera.MaxResolution));
            cmd.Parameters.AddWithValue("@ColorSpaces", MakeIntList(camera.ColorSpaces.Select(x => x.Id).ToList()));

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditCatalogue(int Id, Catalogue catalogue)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblCatalogues] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", catalogue.ShortName);
            cmd.Parameters.AddWithValue("@LongName", catalogue.LongName);

            cmd.ExecuteNonQuery();
        }

        public static int AddCatalogue(Catalogue catalogue)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCatalogues] ([ShortName],[LongName]) " +
                "OUTPUT INSERTED.Id " +
                " VALUES(@ShortName,@LongName)";

            cmd.Parameters.AddWithValue("@ShortName", catalogue.ShortName);
            cmd.Parameters.AddWithValue("@LongName", catalogue.LongName);

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditCollection(int Id, Collection collection)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblCollections] SET " +
                "[CaptureDateTime] = @CaptureDateTime, " +
                "[CatalogueId] = @CatalogueId, " +
                "[ObjectId] = @ObjectId, " +
                "[ObjectTitle] = @ObjectTitle, " +
                "[NumberFrames] = @NumberFrames, " +
                "[FileFormat] = @FileFormat " +
                "[ColorSpace] = @ColorSpace " +
                "[Resolution] = @Resolution " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@CaptureDateTime", collection.CaptureDateTime);
            cmd.Parameters.AddWithValue("@CatalogueId", collection.Catalogue.Id);
            cmd.Parameters.AddWithValue("@ObjectId", collection.ObjectId);
            cmd.Parameters.AddWithValue("@ObjectTitle", collection.ObjectTitle);
            cmd.Parameters.AddWithValue("@NumberFrames", collection.NumberFrames);
            cmd.Parameters.AddWithValue("@FileFormat", collection.FileFormat.Id);
            cmd.Parameters.AddWithValue("@ColorSpace", collection.ColorSpace.Id);
            cmd.Parameters.AddWithValue("@Resolution", MakeResolution(collection.Resolution));

            cmd.ExecuteNonQuery();
        }

        public static int AddCollection(Collection collection)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCollections] ([CaptureDateTime],[CatalogueId],[ObjectId],[ObjectTitle],[NumberFrames],[FileFormat],[ColorSpace],[Resolution]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@CaptureDateTime,@CatalogueId,@ObjectId,@ObjectTitle,@NumberFrames,@FileFormat,@ColorSpace,@Resolution)";

            cmd.Parameters.AddWithValue("@CaptureDateTime", collection.CaptureDateTime);
            cmd.Parameters.AddWithValue("@CatalogueId", collection.Catalogue.Id);
            cmd.Parameters.AddWithValue("@ObjectId", collection.ObjectId);
            cmd.Parameters.AddWithValue("@ObjectTitle", collection.ObjectTitle);
            cmd.Parameters.AddWithValue("@NumberFrames", collection.NumberFrames);
            cmd.Parameters.AddWithValue("@FileFormat", collection.FileFormat.Id);
            cmd.Parameters.AddWithValue("@ColorSpace", collection.ColorSpace.Id);
            cmd.Parameters.AddWithValue("@Resolution", MakeResolution(collection.Resolution));

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public static List<ColorSpace> GetColorSpaces(List<int> Ids = null)
        {
            List<ColorSpace> ColorSpaces = new List<ColorSpace>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
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

        public static void EditColorSpace(int Id, ColorSpace colorSpace)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblColorSpaces] SET " +
                "[Name] = @Name, " +
                "[BitsPerChannel] = @BitsPerChannel, " +
                "[IsMultiChannel] = @IsMultiChannel, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Name", colorSpace.Name);
            cmd.Parameters.AddWithValue("@BitsPerChannel", colorSpace.BitsPerChannel);
            cmd.Parameters.AddWithValue("@IsMultiChannel", colorSpace.IsMultiChannel);

            cmd.ExecuteNonQuery();
        }

        public static int AddColorSpace(ColorSpace colorSpace)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblColorSpaces] ([Name],[BitsPerChannel],[IsMultiChannel]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name,@BitsPerChannel,@IsMultiChannel)";

            cmd.Parameters.AddWithValue("@Name", colorSpace.Name);
            cmd.Parameters.AddWithValue("@BitsPerChannel", colorSpace.BitsPerChannel);
            cmd.Parameters.AddWithValue("@IsMultiChannel", colorSpace.IsMultiChannel);

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditFileFormat(int Id, FileFormat fileFormat)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblFileFormats] SET " +
                "[ShortName] = @ShortName, " +
                "[LongName] = @LongName, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);

            cmd.ExecuteNonQuery();
        }

        public static int AddFileFormat(int Id, FileFormat fileFormat)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblFileFormats] ([ShortName],[LongName]) " +
                "OUPUT INSERTED.Id " +
                "VALUES (@ShortName,@LongName)";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditOptics(int Id, Optic optic)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblOptics] SET " +
                "[Type] = @Type, " +
                "[Value] = @Value, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);

            cmd.ExecuteNonQuery();
        }

        public static int AddOptics(Optic optic)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblOptics] ([Type],[Value]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Type,@Value)";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditPhotographer(int Id, Photographer photographer)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblPhotographers] SET " +
                "[FirstName] = @FirstName, " +
                "[LastName] = @LastName, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LongName", photographer.LastName);

            cmd.ExecuteNonQuery();
        }

        public static int AddPhotographer(Photographer photographer)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblPhotographers] ([FirstName],[LastName]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@FirstName,@LastName)";

            cmd.Parameters.AddWithValue("@ShortName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LongName", photographer.LastName);

            return int.Parse(cmd.ExecuteScalar().ToString());
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

        public static void EditScope(int Id, Scope scope)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblScopes] SET " +
                "[Manufacturer] = @Manufacturer, " +
                "[Name] = @Name, " +
                "[Aperture] = @Aperture, " +
                "[FocalLength] = @FocalLength, " +
                "[CentralObstructionDiameter] = @CentralObstructionDiameter, " +
                "[Robotic] = @Robotic, " +
                "[MountType] = @MountType, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Manufacturer", scope.Manufacturer);
            cmd.Parameters.AddWithValue("@Name", scope.Name);
            cmd.Parameters.AddWithValue("@Aperture", scope.Aperture);
            cmd.Parameters.AddWithValue("@FocalLength", scope.FocalLength);
            cmd.Parameters.AddWithValue("@CentralObstructionDiameter", scope.CentralObstructionDiameter);
            cmd.Parameters.AddWithValue("@Robotic", scope.Robotic);
            cmd.Parameters.AddWithValue("@MountType", (int)scope.MountType);

            cmd.ExecuteNonQuery();
        }

        public static int AddScope(Scope scope)
        {
            SqlConnection con = GetCon();
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

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public static List<Site> GetSites(List<int> Ids = null)
        {
            List<Site> sites = new List<Site>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType] FROM [tblSites] " + selector;

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

        public static void EditSite(int Id, Site site)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "UPDATE [tblSites] SET " +
                "[Name] = @Name, " +
                "[Longtitude] = @Longtitude, " +
                "[LongtitudeType] = @LongtitudeType, " +
                "[Latitude] = @Latitude, " +
                "[LatitudeType] = @LatitudeType, " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);

            cmd.ExecuteNonQuery();
        }

        public static int AddSite(Site site)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INO [tblSites] ([Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name,@Longtitude,@LongtitudeType,@Latitude,@LatitudeType";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        #region Utility Functions
        public static Size ParseResolution(string str)
        {
            List<int> xyData = ParseIntList(str);

            return new Size(xyData[0], xyData[1]);
        }

        public static string MakeResolution(Size size)
        {
            return MakeIntList(new List<int>() { size.Width, size.Height });
        }

        public static List<int> ParseIntList(string str)
        {
            string[] nums = str.Split(';');
            List<int> numList = new List<int>(nums.Length);

            foreach (string num in nums)
                numList.Add(int.Parse(num));

            return numList;
        }

        public static string MakeIntList(List<int> list)
        {
            string res = "";

            foreach (int i in list)
                res += i.ToString() + ";";

            return res.TrimEnd(';');
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

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        #endregion
    }
}
