using AstroDAM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            cmd.Parameters.AddWithValue("@Id", Id);

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
                "[LongName] = @LongName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", catalogue.ShortName);
            cmd.Parameters.AddWithValue("@LongName", catalogue.LongName);
            cmd.Parameters.AddWithValue("@Id", "Id");

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
            List<Collection> collections = new List<Collection>();

            string selector = SelectorBuilder(Ids);

            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT [Id],[CaptureDateTime],[Catalogue],[Object],[ObjectTitle]," +
                "[NumberFrames],[FileFormat],[ColorSpace],[Camera],[Scope],[Site],[Optics]," +
                "[Photographer],[Resolution],[Comments],[FilePath],[MetadataFilePath] " +
                "FROM [tblCollections] " + selector;

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
                Camera camera = GetCameras(new List<int>() { reader.GetInt32(8) })[0];
                Scope scope = GetScopes(new List<int>() { reader.GetInt32(9) })[0];
                Site site = GetSites(new List<int>() { reader.GetInt32(10) })[0];
                List<Optic> optics = GetOptics(ParseIntList(reader.GetString(11)));
                Photographer photographer = GetPhotographers(new List<int>() { reader.GetInt32(12) })[0];
                Size resolution = ParseResolution(reader.GetString(13));
                string comments = reader.GetString(14);
                string filePath = reader.GetString(15);
                string metadataFile = reader.GetString(16);

                collections.Add(new Collection(id, captureDateTime, catalogue, objectId, objectTitle,
                    numberFrames, fileFormat, colorSpace, camera, scope, site, optics, photographer,
                    resolution, comments, filePath, metadataFile));
            }

            return collections;
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
                "[FileFormat] = @FileFormat, " +
                "[ColorSpace] = @ColorSpace, " +
                "[Camera] = @Camera, " +
                "[Scope] = @Scope, " +
                "[Site] = @Site, " +
                "[Optics] = @Optics, " +
                "[Photographer] = @Photographer, " +
                "[Resolution] = @Resolution, " +
                "[Comments] = @Comments, " +
                "[FilePath] = @FilePath, " +
                "[MetadataFilePath] = @MetadataFile " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@CaptureDateTime", collection.CaptureDateTime);
            cmd.Parameters.AddWithValue("@CatalogueId", collection.Catalogue.Id);
            cmd.Parameters.AddWithValue("@ObjectId", collection.Object);
            cmd.Parameters.AddWithValue("@ObjectTitle", collection.ObjectTitle);
            cmd.Parameters.AddWithValue("@NumberFrames", collection.NumberFrames);
            cmd.Parameters.AddWithValue("@FileFormat", collection.FileFormat.Id);
            cmd.Parameters.AddWithValue("@ColorSpace", collection.ColorSpace.Id);
            cmd.Parameters.AddWithValue("@Camera", collection.Camera.Id);
            cmd.Parameters.AddWithValue("@Scope", collection.Scope.Id);
            cmd.Parameters.AddWithValue("@Site", collection.Site.Id);
            cmd.Parameters.AddWithValue("@Optics", MakeIntList(collection.Optics.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Photographer", collection.Photographer.Id);
            cmd.Parameters.AddWithValue("@Resolution", MakeResolution(collection.Resolution));
            cmd.Parameters.AddWithValue("@Comments",collection.Comments);
            cmd.Parameters.AddWithValue("@FilePath", collection.FileName);
            cmd.Parameters.AddWithValue("@MetadataFile", collection.MetaDataFileName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
        }

        public static bool AddCollection(Collection collection)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblCollections] ([CaptureDateTime],[Catalogue],[Object],[ObjectTitle]," +
                "[NumberFrames],[FileFormat],[ColorSpace],[Camera],[Scope],[Site],[Optics]," +
                "[Photographer],[Resolution],[Comments],[FilePath],[MetadataFilePath]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@CaptureDateTime,@CatalogueId,@ObjectId,@ObjectTitle," +
                "@NumberFrames,@FileFormat,@ColorSpace,@Camera,@Scope,@Site,@Optics," +
                "@Photographer,@Resolution,@Comments,@FilePath,@MetadataFile)";

            cmd.Parameters.AddWithValue("@CaptureDateTime", collection.CaptureDateTime);
            cmd.Parameters.AddWithValue("@CatalogueId", collection.Catalogue.Id);
            cmd.Parameters.AddWithValue("@ObjectId", collection.Object);
            cmd.Parameters.AddWithValue("@ObjectTitle", collection.ObjectTitle);
            cmd.Parameters.AddWithValue("@NumberFrames", collection.NumberFrames);
            cmd.Parameters.AddWithValue("@FileFormat", collection.FileFormat.Id);
            cmd.Parameters.AddWithValue("@ColorSpace", collection.ColorSpace.Id);
            cmd.Parameters.AddWithValue("@Camera", collection.Camera.Id);
            cmd.Parameters.AddWithValue("@Scope", collection.Scope.Id);
            cmd.Parameters.AddWithValue("@Site", collection.Site.Id);
            cmd.Parameters.AddWithValue("@Optics", MakeIntList(collection.Optics.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Photographer", collection.Photographer.Id);
            cmd.Parameters.AddWithValue("@Resolution", MakeResolution(collection.Resolution));
            cmd.Parameters.AddWithValue("@Comments",collection.Comments);
            cmd.Parameters.AddWithValue("@FilePath", collection.FileName);
            cmd.Parameters.AddWithValue("@MetadataFile", collection.MetaDataFileName);

            return cmd.ExecuteNonQuery() == 1;
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
            cmd.Parameters.AddWithValue("@Id", Id);

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
                "[LongName] = @LongName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", fileFormat.ShortName);
            cmd.Parameters.AddWithValue("@LongName", fileFormat.LongName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
        }

        public static int AddFileFormat(FileFormat fileFormat)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblFileFormats] ([ShortName],[LongName]) " +
                "OUTPUT INSERTED.Id " +
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
                float value = (float)(float)reader.GetDouble(2);

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
                "[Value] = @Value " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Type", (int)optic.OpticType);
            cmd.Parameters.AddWithValue("@Value", optic.Value);
            cmd.Parameters.AddWithValue("@Id", Id);

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
                "[LastName] = @LastName " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@ShortName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LongName", photographer.LastName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
        }

        public static int AddPhotographer(Photographer photographer)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblPhotographers] ([FirstName],[LastName]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@FirstName,@LastName)";

            cmd.Parameters.AddWithValue("@FirstName", photographer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", photographer.LastName);

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
                float aperture = (float)reader.GetDouble(3);
                float focalLength = (float)reader.GetDouble(4);
                float centralObstructionDiameter = (float)reader.GetDouble(5);
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
                float longtitude = (float)reader.GetDouble(2);
                bool longtitudeType = reader.GetBoolean(3);
                float latitude = (float)reader.GetDouble(4);
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
                "[LatitudeType] = @LatitudeType " +
                "WHERE [Id] = @Id";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
        }

        public static int AddSite(Site site)
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "INSERT INTO [tblSites] ([Name],[Longtitude],[LongtitudeType],[Latitude],[LatitudeType]) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name,@Longtitude,@LongtitudeType,@Latitude,@LatitudeType)";

            cmd.Parameters.AddWithValue("@Name", site.Name);
            cmd.Parameters.AddWithValue("@Longtitude", site.Longtitude);
            cmd.Parameters.AddWithValue("@LongtitudeType", (int)site.LongtitudeType);
            cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
            cmd.Parameters.AddWithValue("@LatitudeType", (int)site.LatitudeType);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        #region Utility Functions
        /// <summary>
        /// Checks whether a specific object is in use in the collections table before deletion.
        /// </summary>
        /// <param name="Type">Object Type</param>
        /// <param name="Id">Object id</param>
        /// <returns></returns>
        public static bool ObjectDeletionChecker(frmManager.ManagerTabs Type, int Id)
        {
            SqlConnection con = GetCon();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT [Id] FROM [tblCollections] ";

            switch (Type)
            {
                case frmManager.ManagerTabs.Cameras:
                    cmd.CommandText = "WHERE [Camera] = @Id";
                    break;
                case frmManager.ManagerTabs.Catalogues:
                    cmd.CommandText = "WHERE [CatalogueId] = @Id";

                    break;
                case frmManager.ManagerTabs.ColorSpaces:
                    cmd.CommandText = "WHERE [ColorSpace] = @Id";

                    break;
                case frmManager.ManagerTabs.FileFormats:
                    cmd.CommandText = "WHERE [FileFormat] = @Id";

                    break;
                case frmManager.ManagerTabs.Optics:
                    cmd.CommandText = "WHERE [Optics] LIKE '%@Id%'";
                    
                    break;
                case frmManager.ManagerTabs.Photographers:
                    cmd.CommandText = "WHERE [Photographer] = @Id";

                    break;
                case frmManager.ManagerTabs.Scopes:
                    cmd.CommandText = "WHERE [Scope] = @Id";

                    break;
                case frmManager.ManagerTabs.Sites:
                    cmd.CommandText = "WHERE [Site] = @Id";

                    break;
                default:
                    break;
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            return cmd.ExecuteReader().HasRows;
        }

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
            if (string.IsNullOrEmpty(str))
                return new List<int>();

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

        public static void PopulateTreeView(ref TreeView treeView, bool isAscending, string format = "")
        {
            SqlConnection con = GetCon();
            SqlCommand cmd = con.CreateCommand();

            // returns dates, ids and object titles of collections
            cmd.CommandText = "SELECT [Id],CAST(FLOOR(CAST([CaptureDateTime] as FLOAT)) as DateTime) FROM tblCollections ORDER BY [CaptureDateTime] " + (isAscending ? "ASC" : "DESC");

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // for every collection, add to a new or existing group - by date.
                int id = reader.GetInt32(0);
                string collectionDate = reader.GetDateTime(1).ToString("yyyy-MM-dd");

                Collection col = GetCollections(new List<int>() { id })[0];

                string posibleKey = "f" + collectionDate; // f is for folder.
                if (!treeView.Nodes.ContainsKey(posibleKey))
                    treeView.Nodes.Add(posibleKey, posibleKey.Substring(1));

                string namingFormat = string.IsNullOrEmpty(format) ? Properties.Preferences.Default.TreeNodeFormat : format;

                foreach (Match match in Regex.Matches(namingFormat, @"\{([^}]+)\}"))
                {
                    // parse the match:
                    string matchVal = match.Value.Replace("{", "").Replace("}", "");

                    string command, parameter = "";
                    if (matchVal.Contains("|"))
                    {
                        command = matchVal.Split('|')[0];
                        parameter = matchVal.Split('|')[1];
                    }
                    else
                        command = matchVal;

                    matchVal = "{" + matchVal + "}";

                    switch (command)
                    {
                        case "dt":
                            namingFormat = namingFormat.Replace(matchVal, col.CaptureDateTime.ToString(parameter));

                            break;

                        case "o":
                            if (parameter == "id")
                                namingFormat = namingFormat.Replace(matchVal, col.Object.ToString());

                            if (parameter == "n")
                                namingFormat = namingFormat.Replace(matchVal, col.ObjectTitle);

                            break;

                        case "c":
                            if (parameter == "id")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.Id.ToString());
                            
                            if (parameter == "sn")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.ShortName);
                            
                            if (parameter == "ln")
                                namingFormat = namingFormat.Replace(matchVal, col.Catalogue.LongName);

                            break;

                        default:
                            break;
                    }
                }
                

                //treeView.Nodes[posibleKey].Nodes.Add("i" + reader.GetString(0), // key. i for item.
                //    reader.GetDateTime(3).ToString("hh:mm:ss") + " - " + reader.GetString(2));

                treeView.Nodes[posibleKey].Nodes.Add("i" + id.ToString(), // key. i for item.
                    namingFormat);

                treeView.ExpandAll();
            }
        }

        #endregion
    }
}
