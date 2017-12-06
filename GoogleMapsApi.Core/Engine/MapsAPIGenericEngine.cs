using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Core.Entities.Common;
using Newtonsoft.Json;

namespace GoogleMapsApi.Core.Engine
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

		private const string AuthenticationFailedMessage = "The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";
        
		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default(CancellationToken))
		{
		    if (request == null)
		        throw new ArgumentNullException("request");

		    var uri = request.GetUri();
		    if (OnUriCreated != null)
		        uri = OnUriCreated(uri);

		    using (var client = new HttpClient { Timeout = timeout })
		    {
		        var response = await client.GetAsync(uri, token);
		        if (!response.IsSuccessStatusCode)
		        {
		            if (response.StatusCode == HttpStatusCode.Forbidden)
		                throw new Exception(AuthenticationFailedMessage);
		            else
		                throw new Exception($"Exception: {response.ToString()}");
		        }

		        var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(data);
            }
		}
    }
}