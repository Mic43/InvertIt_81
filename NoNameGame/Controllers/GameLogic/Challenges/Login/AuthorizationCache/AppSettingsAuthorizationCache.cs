using System;
using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Configuration;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache
{
    public class AppSettingsAuthorizationCache : IMobileServiceAuthorizationCache
    {
        private readonly IValueStorer<string, string> _valueStorer;
        public AppSettingsAuthorizationCache(IValueStorer<string, string> valueStorer)
        {
            _valueStorer = valueStorer;
        }
        public Maybe<AuthorizationCacheData> TryGet()
        {
            var authorizationCacheDataToken = _valueStorer.Read(AppSettingsKeys.AuthorizationCacheDataToken,null);
            var authorizationCacheDataUser = _valueStorer.Read(AppSettingsKeys.AuthorizationCacheDataUser, null);

            if(authorizationCacheDataToken == null || authorizationCacheDataUser == null )          
                return new Maybe<AuthorizationCacheData>();
            return new Maybe<AuthorizationCacheData>(new AuthorizationCacheData(authorizationCacheDataToken, authorizationCacheDataUser));
        }
        public void AddOrUpdate(AuthorizationCacheData authorizationCacheData)
        {
            if (authorizationCacheData == null) throw new ArgumentNullException("authorizationCacheData");

            _valueStorer.Write(AppSettingsKeys.AuthorizationCacheDataToken, authorizationCacheData.AuthToken);
            _valueStorer.Write(AppSettingsKeys.AuthorizationCacheDataUser, authorizationCacheData.UserId);
        }
    }
}