using System;
using Infrastructure;
using Microsoft.WindowsAzure.MobileServices;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; private set; }
        public string AuthToken { get; private set; }
        public string UserId { get; private set; }


        private AuthenticationResult(bool isSuccess, string authToken,string userId)
        {
            if (authToken == null) throw new ArgumentNullException("authToken");
            if (userId == null) throw new ArgumentNullException("userId");

            IsSuccess = isSuccess;
            AuthToken = authToken;
            UserId = userId;
        }
        public static AuthenticationResult CreateFault()
        {
            return new AuthenticationResult(false, string.Empty,string.Empty);
        }
        public static AuthenticationResult CreateSuccess(string authToken, string userId)
        {            
            return new AuthenticationResult(true, authToken, userId);
        }
    }
}