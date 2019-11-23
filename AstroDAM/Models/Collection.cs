using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class Collection
    {
        public Collection() { }

        public Collection(string id, DateTime captureDateTime, Catalogue catalogue, int objectId, 
            string objectTitle, int numberFrames, FileFormat fileFormat, ColorSpace colorSpace, 
            Camera camera, Scope scope, Site site, List<Optic> optics, Photographer photographer, 
            Size resolution, string comments)
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
        }

        [DataMember]
        public string Id { get; set; }

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
    }
}
