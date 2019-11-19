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
        public FileFormat() { }

        public FileFormat(int id, string shortName, string longName)
        {
            Id = id;
            ShortName = shortName;
            LongName = longName;
        }

        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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
