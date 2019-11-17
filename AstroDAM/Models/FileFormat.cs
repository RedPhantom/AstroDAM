using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class FileFormat
    {
        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Such as "SER".
        /// </summary>
        [DataMember]
        public string ShortName { get; set; }

        /// <summary>
        /// Such as "LuCam SER".
        /// </summary>
        [DataMember]
        public string LongName { get; set; }
    }
}
