using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstroDAM.Models
{
    /// <summary>
    /// Represents a camera that can be connected to a scope.
    /// </summary>
    [DataContract]
    public class Camera
    {
        /// <summary>
        /// Initialize a new camera.
        /// </summary>
        public Camera()
        {
            ColorSpaces = new List<ColorSpace>();
        }

        /// <summary>
        /// Initialize a new camera.
        /// </summary>
        /// <param name="id">The unique identifier of the camera in the database.</param>
        /// <param name="shortName">The short, usual name of the camera.</param>
        /// <param name="longName">The long, technical name of the camera.</param>
        /// <param name="maxResolution">The maximum resolution the camera can capture.</param>
        /// <param name="colorSpaces">The color spaces the camera can capture.</param>
        public Camera(int id, string shortName, string longName, Size maxResolution, List<ColorSpace> colorSpaces)
        {
            Id = id;
            ShortName = shortName;
            LongName = longName;
            MaxResolution = maxResolution;
            ColorSpaces = colorSpaces;
        }

        /// <summary>
        /// Unique identifier of the camera in the database.
        /// </summary>
        /// <example>
        /// 0
        /// </example>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// The short, usual name of the camera.
        /// </summary>
        /// <example>
        /// "ASI120MC"
        /// </example>
        [DataMember]
        public string ShortName { get; set; }

        /// <summary>
        /// The long, technical name of the camera.
        /// </summary>
        /// <example>
        /// "Zwo ASI 120MC"
        /// </example>
        [DataMember]
        public string LongName { get; set; }

        /// <summary>
        /// The maximum resolution the camera can capture.
        /// </summary>
        [DataMember]
        public Size MaxResolution { get; set; }

        /// <summary>
        /// The color spaces the camera can capture.
        /// </summary>
        [DataMember]
        public List<ColorSpace> ColorSpaces { get; set; }
    }
}
