using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    /// <summary>
    /// A collection of captured frames (astronomical image).
    /// </summary>
    [DataContract]
    public class Collection
    {
        /// <summary>
        /// Initialize a new collection.
        /// </summary>
        public Collection() { }

        /// <summary>
        /// Initialize a new collection.
        /// </summary>
        /// <param name="id">Unique identifier of the collection in the database.</param>
        /// <param name="captureDateTime">Date and time of capturing the first frame in the collection.</param>
        /// <param name="catalogue">Catalogue of the object captured in this collection.</param>
        /// <param name="objectId">Unique identifier of the object in the catalogue.</param>
        /// <param name="objectTitle">Name of the object captured in this collection.</param>
        /// <param name="numberFrames">Number of frames captured in this catalogue.</param>
        /// <param name="fileFormat">Format of the file that contains the captured data.</param>
        /// <param name="colorSpace">Color space of the file that contains the captured data.</param>
        /// <param name="camera">Camera that captured this collection.</param>
        /// <param name="scope">Telescope used to capture this collection.</param>
        /// <param name="site">Physical site in which the collection was captured.</param>
        /// <param name="optics">Optical peripherals used to capture the collection.</param>
        /// <param name="photographer">Photographer that captured the collection.</param>
        /// <param name="resolution"></param>
        /// <param name="comments"></param>
        /// <param name="fileName"></param>
        /// <param name="metadataFile"></param>
        public Collection(int id, DateTime captureDateTime, Catalogue catalogue, int objectId, 
            string objectTitle, int numberFrames, FileFormat fileFormat, ColorSpace colorSpace, 
            Camera camera, Scope scope, Site site, List<Optic> optics, Photographer photographer, 
            Size resolution, string comments, string fileName, string metadataFile)
        {
            Id = id;
            CaptureDateTime = captureDateTime;
            Catalogue = catalogue;
            Object = objectId;
            ObjectTitle = objectTitle;
            NumberFrames = numberFrames;
            FileFormat = fileFormat;
            ColorSpace = colorSpace;
            Camera = camera;
            Scope = scope;
            Site = site;
            Optics = optics;
            Photographer = photographer;
            Resolution = resolution;
            Comments = comments;
            FileName = fileName;
            MetaDataFileName = metadataFile;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CaptureDateTime { get; set; }

        [DataMember]
        public Catalogue Catalogue { get; set; }

        [DataMember]
        public int Object { get; set; }

        [DataMember]
        public string ObjectTitle { get; set; }

        [DataMember]
        public int NumberFrames { get; set; }

        [DataMember]
        public FileFormat FileFormat { get; set; }

        [DataMember]
        public ColorSpace ColorSpace { get; set; }

        [DataMember]
        public Camera Camera { get; set; }

        [DataMember]
        public Scope Scope { get; set; }

        [DataMember]
        public Site Site { get; set; }

        [DataMember]
        public List<Optic> Optics { get; set; }

        [DataMember]
        public Photographer Photographer { get; set; }

        [DataMember]
        public Size Resolution { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string MetaDataFileName { get; set; }
    }
}
