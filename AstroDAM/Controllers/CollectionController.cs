using System;
using System.Linq;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections.Generic;
using AstroDAM.Models;

namespace AstroDAM.Controllers
{
    public static class CollectionController
    {
        public static List<Collection> GetCollections(List<int> Ids = null)
        {
            List<Collection> collections = new List<Collection>();

            string selector = DbManager.SelectorBuilder(Ids);

            SqlConnection con = DbManager.GetConnection();
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
                Catalogue catalogue = CatalogueController.GetCatalogues(new List<int>() { reader.GetInt32(2) })[0];
                int objectId = reader.GetInt32(3);
                string objectTitle = reader.GetString(4);
                int numberFrames = reader.GetInt32(5);
                FileFormat fileFormat = FileFormatController.GetFileFormats(new List<int>() { reader.GetInt32(6) })[0];
                ColorSpace colorSpace = ColorSpaceController.GetColorSpaces(new List<int>() { reader.GetInt32(7) })[0];
                Camera camera = CameraController.GetCameras(new List<int>() { reader.GetInt32(8) })[0];
                Scope scope = ScopeController.GetScopes(new List<int>() { reader.GetInt32(9) })[0];
                Site site = SiteController.GetSites(new List<int>() { reader.GetInt32(10) })[0];
                List<Optic> optics = OpticsController.GetOptics(Utilities.StringToIntList(reader.GetString(11)));
                Photographer photographer = PhotographerController.GetPhotographers(new List<int>() { reader.GetInt32(12) })[0];
                Size resolution = Utilities.StringToResolution(reader.GetString(13));
                string comments = reader.GetString(14);
                string filePath = reader.GetString(15);
                string metadataFile = reader.GetString(16);

                collections.Add(new Collection(id, captureDateTime, catalogue, objectId, objectTitle,
                    numberFrames, fileFormat, colorSpace, camera, scope, site, optics, photographer,
                    resolution, comments, filePath, metadataFile));
            }

            con.Close();
            return collections;
        }

        public static void DeleteCollections(List<int> Ids = null)
        {
            string selector = DbManager.SelectorBuilder(Ids, false);

            SqlConnection con = DbManager.GetConnection();
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM [tblCollections] " + selector;

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void EditCollection(int Id, Collection collection)
        {
            SqlConnection con = DbManager.GetConnection();
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
            cmd.Parameters.AddWithValue("@Optics", Utilities.IntListToString(collection.Optics.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Photographer", collection.Photographer.Id);
            cmd.Parameters.AddWithValue("@Resolution", Utilities.ResolutionToString(collection.Resolution));
            cmd.Parameters.AddWithValue("@Comments", collection.Comments);
            cmd.Parameters.AddWithValue("@FilePath", collection.FileName);
            cmd.Parameters.AddWithValue("@MetadataFile", collection.MetaDataFileName);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static bool AddCollection(Collection collection)
        {
            SqlConnection con = DbManager.GetConnection();
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
            cmd.Parameters.AddWithValue("@Optics", Utilities.IntListToString(collection.Optics.Select(x => x.Id).ToList()));
            cmd.Parameters.AddWithValue("@Photographer", collection.Photographer.Id);
            cmd.Parameters.AddWithValue("@Resolution", Utilities.ResolutionToString(collection.Resolution));
            cmd.Parameters.AddWithValue("@Comments", collection.Comments);
            cmd.Parameters.AddWithValue("@FilePath", collection.FileName);
            cmd.Parameters.AddWithValue("@MetadataFile", collection.MetaDataFileName);

            bool res = cmd.ExecuteNonQuery() == 1;
            con.Close();
            return res;
        }
    }
}
