using System.Runtime.Serialization;
using GoogleMapsApi.Core.Entities.Common;

namespace GoogleMapsApi.Core.Entities.PlacesNearBy.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    [DataContract]
    public class Geometry
    {
        [DataMember(Name = "location")]
        public Location Location { get; set; } 
    }
}
