using Infrastructure;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache
{
    public interface IMobileServiceAuthorizationCache
    {
        Maybe<AuthorizationCacheData> TryGet();
        void AddOrUpdate(AuthorizationCacheData mobileServiceUser);

    }
}