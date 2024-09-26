using System;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.MobileServices;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache
{ 
    public class AuthorizationCacheData
    {
      
        public string UserId { get; private set; }
       
        public string AuthToken { get; private set; }
    
        public AuthorizationCacheData(string authToken, string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (authToken == null) throw new ArgumentNullException("authToken");
            UserId = userId;
            AuthToken = authToken;
          //  MobileServiceAuthenticationProvider = mobileServiceAuthenticationProvider;
        }
    }
}