using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    [DataContract]
    public class Photographer
    {
        public Photographer() { }

        public Photographer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Such as 0.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}
