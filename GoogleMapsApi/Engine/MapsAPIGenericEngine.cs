using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using System.Net.Http;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReciviedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReciviedDelegate OnRawResponseRecivied;
        
		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		private const string AuthenticationFailedMessage =
			"The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";

		static MapsAPIGenericEngine()
		{
			var baseUrl = new TRequest().BaseUrl;
			//HttpServicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + baseUrl));
			//HttpsServicePoint = ServicePointManager.FindServicePoint(new Uri("https://" + baseUrl));
		}

		protected internal static TResponse QueryGoogleAPI(TRequest request, TimeSpan timeout)
		{
            return QueryGoogleAPIAsync(request, timeout).Result;
        }        
		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException("request");
            
            var uri = request.GetUri();
            if (OnUriCreated != null)
                uri = OnUriCreated(uri);

            using (var client = new HttpClient {Timeout = timeout})
            {
                var response = await client.GetAsync(uri, token);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        throw new Exception(AuthenticationFailedMessage);
                    else
                        throw new Exception($"Exception: {response.ToString()}");
                }

                var data = await response.Content.ReadAsByteArrayAsync();
                OnRawResponseRecivied?.Invoke(data);
                return await Task.FromResult(Deserialize(data));
            }
		}
		private static TResponse Deserialize(byte[] serializedObject)
		{
			var serializer = new DataContractJsonSerializer(typeof(TResponse));
			var stream = new MemoryStream(serializedObject, false);
            return (TResponse)serializer.ReadObject(stream);
		}
	}
}