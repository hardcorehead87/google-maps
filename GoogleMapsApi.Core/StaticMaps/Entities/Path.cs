using System.Collections.Generic;
using GoogleMapsApi.Core.Entities.Common;

namespace GoogleMapsApi.Core.StaticMaps.Entities
{
	public class Path
	{
		public PathStyle Style { get; set; }

		public IList<ILocationString> Locations { get; set; }
	}
}