using GoogleMapsApi.Core.Entities.Directions.Request;
using GoogleMapsApi.Core.Entities.Directions.Response;
using GoogleMapsApi.Core.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Core.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Core.Entities.Elevation.Request;
using GoogleMapsApi.Core.Entities.Elevation.Response;
using GoogleMapsApi.Core.Entities.Geocoding.Request;
using GoogleMapsApi.Core.Entities.Geocoding.Response;
using GoogleMapsApi.Core.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Core.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Core.Entities.Places.Request;
using GoogleMapsApi.Core.Entities.Places.Response;
using GoogleMapsApi.Core.Entities.PlacesDetails.Request;
using GoogleMapsApi.Core.Entities.PlacesDetails.Response;
using GoogleMapsApi.Core.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Core.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Core.Entities.PlacesRadar.Request;
using GoogleMapsApi.Core.Entities.PlacesRadar.Response;
using GoogleMapsApi.Core.Entities.PlacesText.Request;
using GoogleMapsApi.Core.Entities.PlacesText.Response;
using GoogleMapsApi.Core.Entities.TimeZone.Request;
using GoogleMapsApi.Core.Entities.TimeZone.Response;

namespace GoogleMapsApi.Core
{
    public class GoogleMaps
	{
		/// <summary>Perform geocoding operations.</summary>
		public static IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode
		{
			get
			{
				return EngineFacade<GeocodingRequest, GeocodingResponse>.Instance;
			}
		}
		/// <summary>Perform directions operations.</summary>
		public static IEngineFacade<DirectionsRequest, DirectionsResponse> Directions
		{
			get
			{
				return EngineFacade<DirectionsRequest, DirectionsResponse>.Instance;
			}
		}
		/// <summary>Perform elevation operations.</summary>
		public static IEngineFacade<ElevationRequest, ElevationResponse> Elevation
		{
			get
			{
				return EngineFacade<ElevationRequest, ElevationResponse>.Instance;
			}
		}

		/// <summary>Perform places operations.</summary>
		public static IEngineFacade<PlacesRequest, PlacesResponse> Places
		{
			get
			{
				return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
			}
		}

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText
        {
            get
            {
                return EngineFacade<PlacesTextRequest, PlacesTextResponse>.Instance;
            }
        }

        /// <summary>Perform places radar search operations.</summary>
        public static IEngineFacade<PlacesRadarRequest, PlacesRadarResponse> PlacesRadar
        {
            get
            {
                return EngineFacade<PlacesRadarRequest, PlacesRadarResponse>.Instance;
            }
        }

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone
        {
            get
            {
                return EngineFacade<TimeZoneRequest, TimeZoneResponse>.Instance;
            }
        }

        /// <summary>Perform places details  operations.</summary>
        public static IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails
        {
            get
            {
                return EngineFacade<PlacesDetailsRequest, PlacesDetailsResponse>.Instance;
            }
        }

        /// <summary>Perform place autocomplete operations.</summary>
        public static IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete
        {
            get
            {
                return EngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse>.Instance;
            }
        }

        /// <summary>Perform near by places operations.</summary>
        public static IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy
        {
            get
            {
                return EngineFacade<PlacesNearByRequest, PlacesNearByResponse>.Instance;
            }
        }
        /// <summary>Retrieve duration and distance values based on the recommended route between start and end points.</summary>
        public static IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix
        {
            get
            {
                return EngineFacade<DistanceMatrixRequest, DistanceMatrixResponse>.Instance;
            }
        }

    }
}
