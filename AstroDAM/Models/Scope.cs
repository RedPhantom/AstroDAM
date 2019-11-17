using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    class Scope
    {
        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Manufacturer { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Aperture { get; set; }

        [DataMember]
        public double FocalLength { get; set; }

        [DataMember]
        public double CentralObstructionDiameter { get; set; }

        [DataMember]
        public bool Robotic { get; set; }

        [DataMember]
        public MountTypes MountType { get; set; }

        public enum MountTypes
        {
            AltAz,
            EqNorth,
            EqSouth
        }
    }
}
