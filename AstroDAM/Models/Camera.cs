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
    public class Camera
    {
        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Such as "ASI120MC".
        /// </summary>
        [DataMember]
        public string ShortName { get; set; }

        /// <summary>
        /// Such as "Zwo ASI 120MC".
        /// </summary>
        [DataMember]
        public string LongName { get; set; }

        [DataMember]
        public Size MaxResolution { get; set; }

        [DataMember]
        public List<ColorSpace> ColorSpaces { get; set; }
    }
}
