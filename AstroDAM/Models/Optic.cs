using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    class Optic
    {
        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public OpticTypes OpticType { get; set; }

        /// <summary>
        /// Focal length modifier or focal length.
        /// </summary>
        [DataMember]
        public double Value { get; set; }

        public enum OpticTypes
        {
            Reducer,
            Barlow,
            Galilean,
            Convex,
            Huygens,
            Ramsden,
            Kellner,
            Plossl,
            Orthoscopic,
            Monocentric,
            Erfle,
            Konig,
            RKE,
            Nagler
        }
    }
}
