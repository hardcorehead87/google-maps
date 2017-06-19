using System.Runtime.Serialization;

namespace GoogleMapsApi.Core.Entities.DistanceMatrix.Request
{
    [DataContract]
    public enum DistanceMatrixUnitSystems
    {
        [EnumMember]
        metric, // kilometers an meters.
        [EnumMember]
        imperial, // miles and feet.
    }
}
