using System;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Configuration;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
    class StoreToCacheAuthTokenProviderFactory : IAuthTokenProviderFactory
    {
        private readonly IMobileServiceAuthorizationCache _authorizationCache;
        public StoreToCacheAuthTokenProviderFactory(IMobileServiceAuthorizationCache authorizationCache)
        {
            if (authorizationCache == null) throw new ArgumentNullException("authorizationCache");
            _authorizationCache = authorizationCache;
        }
//        public IAuthenticator Create(MobileServiceAuthenticationProvider currentProvider)
//        {
//            IAuthenticator innerAuth;
//            switch (currentProvider)
//            {
//                case MobileServiceAuthenticationProvider.MicrosoftAccount:
//                    innerAuth = new MicrosoftAuthenticator(Constants.MsClientId,mobileServiceClient);
//                    break;
//                default:
//                    throw new NotSupportedException("provider is not supported!");
//
//            }
//            return new StoreToCacheAuthenticator(innerAuth, _authorizationCache);
//        }
//        public IAuthenticator Create(MobileServiceAuthenticationProvider currentProvider)
//        {
//            return Create(currentProvider,
//                new MobileServiceClient(Constants.InvertItServiceAddress, Constants.InvertItServiceAppKey));
//        }
        public IAuthTokenProvider Create(MobileServiceAuthenticationProvider currentProvider)
        {
            return null;
//             switch (currentProvider)
//            {
//                case MobileServiceAuthenticationProvider.MicrosoftAccount:
//                    innerAuth = new (Constants.MsClientId,mobileServiceClient);
//                    break;
//                default:
//                    throw new NotSupportedException("provider is not supported!");
//
//            }
//            return new StoreToCacheAuthenticator(innerAuth, _authorizationCache);
        }
    }
}