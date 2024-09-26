using System;
using Infrastructure.Storage;
using NoNameGame.Configuration;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
//    public class FromCacheTokenProvider : IAuthTokenProvider
//    {
//        private readonly IValueStorer<string, string> _valueStorer;
//        public FromCacheTokenProvider(IValueStorer<string, string> valueStorer)
//        {
//            if (valueStorer == null) throw new ArgumentNullException("valueStorer");
//            _valueStorer = valueStorer;
//        }
//
//        public AuthenticationToken Get()
//        {
//            string authToken = _valueStorer.Read(AppSettingsKeys.AuthorizationCacheData,null);
//            return authToken == null ? AuthenticationToken.CreateFault() : AuthenticationToken.CreateSuccess(authToken);
//        }
//    }
}