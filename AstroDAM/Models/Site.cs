using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    class Site
    {
        [DataMember]
        public int ID { get; set; }

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
        public double Latitue { get; set; }

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
