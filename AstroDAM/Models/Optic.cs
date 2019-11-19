using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class Optic
    {
        public Optic() { }

        public Optic(int id, OpticTypes opticType, double value)
        {
            Id = id;
            OpticType = opticType;
            Value = value;
        }

        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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
            Nagler,
            Other
        }
    }
}
