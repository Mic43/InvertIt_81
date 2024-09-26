using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;

namespace NoNameGame.Helpers.Network
{
//    class TokenExpiryHandler : DelegatingHandler
//    {
//        private readonly MobileServiceClient _mobileServiceClient;
//
//        private IAuthTokenProvider _authTokenProvider;
//
//        public IAuthTokenProvider AuthTokenProvider
//        {
//            get { return _authTokenProvider; }
//            set
//            {
//                if(value == null)
//                    throw new ArgumentNullException("value");
//                _authTokenProvider = value;
//            }
//        }
//        public MobileServiceAuthenticationProvider MobileServiceAuthenticationProvider { get; set; }
//        public TokenExpiryHandler(MobileServiceClient mobileServiceClient, MobileServiceAuthenticationProvider mobileServiceAuthenticationProvider, IAuthTokenProvider authTokenProvider)
//        {
//            if (mobileServiceClient == null) throw new ArgumentNullException("mobileServiceClient");
//            if (authTokenProvider == null) throw new ArgumentNullException("authTokenProvider");
//
//            MobileServiceAuthenticationProvider = mobileServiceAuthenticationProvider;
//            _mobileServiceClient = mobileServiceClient;
//            _authTokenProvider = authTokenProvider;
//        }
//
//        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            var clonedRequest = await CloneRequest(request);
//            var response = await base.SendAsync(clonedRequest, cancellationToken);
//
//            if (response.StatusCode != HttpStatusCode.Unauthorized) return response;
//            // Oh noes, user is not logged in - we got a 401
//            // Log them in, this time hardcoded with Facebook but you would
//            // trigger the login presentation in your application
//
//
//
//            var authenticateResult = _authTokenProvider.Get();
//            
//
//
//
//            if (!authenticateResult.IsSuccess)
//                return response;
//
//            var result = await _mobileServiceClient.LoginAsync(MobileServiceAuthenticationProvider,
//                JObject.FromObject(new {access_token = authenticateResult.AuthToken}));
//
//            // Clone the request
//            clonedRequest = await CloneRequest(request);
//            clonedRequest.Headers.Remove("X-ZUMO-AUTH");
//            // Set the authentication header
//            clonedRequest.Headers.Add("X-ZUMO-AUTH", result.MobileServiceAuthenticationToken);
//
//            // Resend the request
//            response = await base.SendAsync(clonedRequest, cancellationToken);
//            return response;
//        }
//
//        private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
//        {
//            var result = new HttpRequestMessage(request.Method, request.RequestUri);
//            foreach (var header in request.Headers)
//            {
//                result.Headers.Add(header.Key, header.Value);
//            }
//
//            if (request.Content != null && request.Content.Headers.ContentType != null)
//            {
//                var requestBody = await request.Content.ReadAsStringAsync();
//                var mediaType = request.Content.Headers.ContentType.MediaType;
//                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
//                foreach (var header in request.Content.Headers)
//                {
//                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
//                    {
//                        result.Content.Headers.Add(header.Key, header.Value);
//                    }
//                }
//            }
//
//            return result;
//        }
//    }
}