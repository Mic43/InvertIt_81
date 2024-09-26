using System;
using System.Linq;
using System.Threading.Tasks;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
//    public class StoreToCacheAuthenticator:IAuthenticator
//    {
//        private readonly IAuthenticator _authenticator;
//        private readonly IMobileServiceAuthorizationCache  _authorizationCache;
//        public StoreToCacheAuthenticator(IAuthenticator authenticator, IMobileServiceAuthorizationCache authorizationCache)
//        {
//            if (authenticator == null) throw new ArgumentNullException("authenticator");
//            if (_authorizationCache == null) throw new ArgumentNullException("authenticator");
//            
//            _authenticator = authenticator;
//            _authorizationCache = authorizationCache;
//        }
//        public async Task<AuthenticationToken> Authenticate()
//        {            
//            var result = await _authenticator.Authenticate();
//            if (result.IsSuccess)
//            {
//                var mobileServiceUser = result.MobileServiceUser.Single();
//                _authorizationCache.AddrUpdate(new AuthorizationCacheData(mobileServiceUser.UserId, 
//                    mobileServiceUser.MobileServiceAuthenticationToken,result.CurrentProvider.Value));
//            }
//            return result;
//        }
//    }
}