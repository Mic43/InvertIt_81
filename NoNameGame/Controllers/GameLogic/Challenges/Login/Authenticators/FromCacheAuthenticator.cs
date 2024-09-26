using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
//    public class FromCacheAuthenticator : IAuthenticator
//    {
//  
//        private readonly IMobileServiceAuthorizationCache _mobileServiceAuthorizationCache;
//        private readonly MobileServiceClient _client;
//        public FromCacheAuthenticator( IMobileServiceAuthorizationCache mobileServiceAuthorizationCache, MobileServiceClient client)
//        {
//            
//            if (mobileServiceAuthorizationCache == null) throw new ArgumentNullException("mobileServiceAuthorizationCache");
//            if (client == null) throw new ArgumentNullException("client");           
//            _mobileServiceAuthorizationCache = mobileServiceAuthorizationCache;
//            _client = client;
//        }
//        public async Task<AuthenticationToken> Authenticate()
//        {
//            var cachedValue = _mobileServiceAuthorizationCache.TryGet();
//            if (cachedValue.HasValue)
//            {
//                var cachedAuthenticationResult = cachedValue.Single();
//                _client.CurrentUser = new MobileServiceUser(cachedAuthenticationResult.UserId) {MobileServiceAuthenticationToken = cachedAuthenticationResult.AuthToken};
//                return await Task.FromResult(AuthenticationToken.CreateSuccess(_client.CurrentUser, cachedAuthenticationResult.MobileServiceAuthenticationProvider));
//            }
//            return await Task.FromResult(AuthenticationToken.CreateFault());
//        }
//    }
}