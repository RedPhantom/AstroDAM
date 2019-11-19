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

        public Collection(int id, DateTime captureDateTime, Catalogue catalogue, int objectId, string objectTitle, int numberFrames, FileFormat fileFormat, ColorSpace colorSpace, Size resolution)
        {
            Id = id;
            CaptureDateTime = captureDateTime;
            Catalogue = catalogue;
            ObjectId = objectId;
            ObjectTitle = objectTitle;
            NumberFrames = numberFrames;
            FileFormat = fileFormat;
            ColorSpace = colorSpace;
            Resolution = resolution;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CaptureDateTime { get; set; }

        [DataMember]
        public Catalogue Catalogue { get; set; }

        [DataMember]
        public int ObjectId { get; set; }

        [DataMember]
        public string ObjectTitle { get; set; }

        [DataMember]
        public int NumberFrames { get; set; }

        [DataMember]
        public FileFormat FileFormat { get; set; }

        [DataMember]
        public ColorSpace ColorSpace { get; set; }

        [DataMember]
        public Size Resolution { get; set; }
    }
}
