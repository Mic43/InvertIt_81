using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ServiceDTOs.Login;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
    public class CustomAuthenticator : IAuthenticator
    {
        private readonly MobileServiceClient _mobileServiceClient;
       
        public CustomAuthenticator(MobileServiceClient mobileServiceClient)
        {
            if (mobileServiceClient == null) throw new ArgumentNullException("mobileServiceClient");
            _mobileServiceClient = mobileServiceClient;
          
        }
    
        public async Task<AuthenticationResult> Authenticate(string userName, string password)
        {

            try
            {
                var result = await _mobileServiceClient.
                    InvokeApiAsync<LoginRequest, CustomLoginResult>("customLogin",
                        new LoginRequest(userName, password));
                return AuthenticationResult.CreateSuccess(result.MobileServiceAuthenticationToken, result.UserId);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                return AuthenticationResult.CreateFault();
            }          
        }
    }
}