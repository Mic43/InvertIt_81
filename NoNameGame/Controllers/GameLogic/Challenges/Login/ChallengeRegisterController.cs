using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.ChallengePages;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
    public class ChallengeRegisterController
    {
        private readonly MobileServiceClient _mobileServiceClient;
        public ChallengeRegisterController(MobileServiceClient mobileServiceClient)
        {
            _mobileServiceClient = mobileServiceClient;
        }
        public async Task<bool> TryRegisterAsync(string userName,string password)
        {
            try
            {

                var result = await _mobileServiceClient.
                    InvokeApiAsync<RegisterPage.RegistrationRequest, string>("customRegistration",
                        new RegisterPage.RegistrationRequest(userName, password));


                return true;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                return false;
            }
        }

//        private readonly ICurrentMobileClientAutheneticationProviderSetter _autheneticationProviderSetter;
//        private readonly ICurrentMobileClientFactory _mobileClientFactory;
//        private readonly IAuthTokenProviderFactory _authTokenProviderFactory;
//
//      
//        public async Task<bool> TryRegisterMicrosoft()
//        {
//            var authTokenProvider = _authTokenProviderFactory.Create(MobileServiceAuthenticationProvider.MicrosoftAccount);
//            var authenticationToken = authTokenProvider.Get();
//
//            if (!authenticationToken.IsSuccess)
//                return false;
//
//            await _mobileClientFactory.Create().LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount,
//                JObject.FromObject(new
//                {
//                    auth_Token = authenticationToken.AuthToken
//                }));
//            _autheneticationProviderSetter.Set((MobileServiceAuthenticationProvider.MicrosoftAccount));
//            return true;
//        }
    }
}