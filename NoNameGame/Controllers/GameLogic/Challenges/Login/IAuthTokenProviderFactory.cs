using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
    public interface IAuthTokenProviderFactory
    {        
        IAuthTokenProvider Create(MobileServiceAuthenticationProvider currentProvider);
    }
}