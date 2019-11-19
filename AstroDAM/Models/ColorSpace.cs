using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class ColorSpace
    {
        public ColorSpace()
        {

        }

        public ColorSpace(int Id, string Name, byte BitsPerChannel, bool IsMultiChannel)
        {
            this.Id = Id;
            this.Name = Name;
            this.BitsPerChannel = BitsPerChannel;
            this.IsMultiChannel = IsMultiChannel;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte BitsPerChannel { get; set; }

        [DataMember]
        public bool IsMultiChannel { get; set; }
    }
}
