using System.Threading.Tasks;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
    public interface IAuthenticator
    {
        Task<AuthenticationResult> Authenticate(string userName, string password);
    }
}