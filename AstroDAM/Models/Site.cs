using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class Site
    {
        public Site(int iD, string name, double longtitude, LongtitudeTypes longtitudeType, double latitude, LatitudeTypes latitudeType)
        {
            Id = iD;
            Name = name;
            Longtitude = longtitude;
            LongtitudeType = longtitudeType;
            Latitude = latitude;
            LatitudeType = latitudeType;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// In the decimal form.
        /// </summary>
        [DataMember]
        public double Longtitude { get; set; }

        [DataMember]
        public LongtitudeTypes LongtitudeType { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public LatitudeTypes LatitudeType { get; set; }

        public enum LatitudeTypes
        {
            East,
            West
        }

        public enum LongtitudeTypes
        {
            North,
            South
        }
    }
}
