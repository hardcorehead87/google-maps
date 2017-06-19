using System.Runtime.Serialization;

namespace GoogleMapsApi.Core.Entities.DistanceMatrix.Request
{
    [DataContract]
    public enum DistanceMatrixRestrictions
    {
        [EnumMember]
        tolls,
        [EnumMember]
        highways,
        [EnumMember]
        ferries,
        [EnumMember]
        indoor,
    }
}
