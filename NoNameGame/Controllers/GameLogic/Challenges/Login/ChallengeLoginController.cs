using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;
using ServiceDTOs.Login;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
    public class ChallengeLoginController
    {
        private readonly IAuthenticator _authenticator;
        private readonly IMobileServiceAuthorizationCache _mobileServiceAuthorizationCache;
        private readonly IMobileServiceClient _mobileServiceClient;
        public ChallengeLoginController(IAuthenticator authenticator, IMobileServiceAuthorizationCache mobileServiceAuthorizationCache, 
            IMobileServiceClient mobileServiceClient)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");
            if (mobileServiceAuthorizationCache == null)
                throw new ArgumentNullException("mobileServiceAuthorizationCache");
            if (mobileServiceClient == null) throw new ArgumentNullException("mobileServiceClient");

            _authenticator = authenticator;
            _mobileServiceAuthorizationCache = mobileServiceAuthorizationCache;
            _mobileServiceClient = mobileServiceClient;
        }
        public bool IsAuthTokenCached()
        {
            return _mobileServiceAuthorizationCache.TryGet().HasValue;
        }
        public void LoginWithCachedAuthToken()
        {
            var authorizationCacheData = _mobileServiceAuthorizationCache.TryGet();
            if (!authorizationCacheData.HasValue)
                throw new InvalidOperationException(
                    "There is no cached AuthToken! Use IsAuthTokenCached function to check before calling!");

            var cacheData = authorizationCacheData.Single();
            _mobileServiceClient.CurrentUser = new MobileServiceUser(cacheData.UserId) { MobileServiceAuthenticationToken = cacheData.AuthToken };   
        }
        public async Task<RegistrationResponse> TryRegisterAsync(string userName, string password)
        {           
            var result = await _mobileServiceClient.
                InvokeApiAsync<RegistrationRequest, RegistrationResponse>("customRegistration",
                    new RegistrationRequest(userName, password));


            return result;
          
        }
        public async Task<bool> TryLoginAsync(string userName, string password)
        {
            var result = await _authenticator.Authenticate(userName,password);
            if (!result.IsSuccess) return false;

            _mobileServiceAuthorizationCache.AddOrUpdate(new AuthorizationCacheData(result.AuthToken,result.UserId));
            _mobileServiceClient.CurrentUser = new MobileServiceUser(result.UserId) {MobileServiceAuthenticationToken = result.AuthToken};   
            return true;
        }
    }
}