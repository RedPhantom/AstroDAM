using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    /// <summary>
    /// Represents a catalogue of space objects (stars, nebulas etc.)
    /// </summary>
    [DataContract]
    public class Catalogue
    {
        /// <summary>
        /// Initialize a new catalogue.
        /// </summary>
        public Catalogue() { }

        /// <summary>
        /// Initialize a new catalogue.
        /// </summary>
        /// <param name="id">Unique identifier of the camera in the database.</param>
        /// <param name="shortName">The short, abbreviated name of the catalogue.</param>
        /// <param name="longName">The long, technical name of the catalogue.</param>
        public Catalogue(int id, string shortName, string longName)
        {
            Id = id;
            ShortName = shortName;
            LongName = longName;
        }

        /// <summary>
        /// Unique identifier of the catalogue in the database.
        /// </summary>
        /// <example>
        /// 0
        /// </example>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// The short, abbreviated name of the camera.
        /// </summary>
        /// <example>
        /// "M"
        /// </example>
        [DataMember]
        public string ShortName { get; set; }

        /// <summary>
        /// The long, technical name of the catalogue.
        /// </summary>
        /// <example>
        /// "Messier"
        /// </example>
        [DataMember]
        public string LongName { get; set; }
    }
}
