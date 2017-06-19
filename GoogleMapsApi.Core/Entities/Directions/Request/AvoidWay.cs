using System;

namespace GoogleMapsApi.Core.Entities.Directions.Request
{
	[Flags]
	public enum AvoidWay
	{
		Nothing = 0x0,
		Tolls = 0x1,
		Highways = 0x2
	}
}
