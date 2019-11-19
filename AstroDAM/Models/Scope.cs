using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class Scope
    {
        public Scope() { }

        public Scope(int id, string manufacturer, string name, double aperture, double focalLength, double centralObstructionDiameter, bool robotic, MountTypes mountType)
        {
            Id = id;
            Manufacturer = manufacturer;
            Name = name;
            Aperture = aperture;
            FocalLength = focalLength;
            CentralObstructionDiameter = centralObstructionDiameter;
            Robotic = robotic;
            MountType = mountType;
        }

        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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
