using System;
using System.Threading.Tasks;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators
{
    class MsAuthTokenProvider
    {
        private readonly string _clientId;
        public async Task<AuthenticationToken> GetToken()
        {

            LiveAuthClient liveIdClient = new LiveAuthClient(_clientId);
            LiveLoginResult result = null;
            try
            {
                result = await liveIdClient.LoginAsync(new[] { "wl.basic" });
            }
            catch (LiveAuthException)
            {
                return AuthenticationToken.CreateFault();
            }

            if (result.Status != LiveConnectSessionStatus.Connected)
                return AuthenticationToken.CreateFault();

            return AuthenticationToken.CreateSuccess(result.Session.AuthenticationToken);
        }
    }

//    public class MicrosoftAuthenticator : IAuthenticator
//    {
//        private readonly string _clientId;
//        private readonly MobileServiceClient _mobileServiceClient;
//        public MicrosoftAuthenticator(string clientId,MobileServiceClient mobileServiceClient)
//        {
//            if (clientId == null) throw new ArgumentNullException("clientId");
//            if (mobileServiceClient == null) throw new ArgumentNullException("mobileServiceClient");
//            _clientId = clientId;
//            _mobileServiceClient = mobileServiceClient;
//        }
//        public async Task<AuthenticationResult> Authenticate()
//        {            
//            LiveAuthClient liveIdClient = new LiveAuthClient(_clientId);
//            LiveLoginResult result = null;
//            try
//            {                
//                result = await liveIdClient.LoginAsync(new[] { "wl.basic" });               
//            }
//            catch (LiveAuthException)
//            {
//                return AuthenticationResult.CreateFault();
//            }
//
//            if (result.Status != LiveConnectSessionStatus.Connected)            
//                return AuthenticationResult.CreateFault();
//            
//
//            MobileServiceUser loginResult;
//            try
//            {
//                _mobileServiceClient.LoginAsync()
//                loginResult =  await _mobileServiceClient
//                    .LoginWithMicrosoftAccountAsync(result.Session.AuthenticationToken);
//            }
//            catch (Exception)
//            {
//                return AuthenticationResult.CreateFault();
//            }
//
//            return AuthenticationResult.CreateSuccess(loginResult,MobileServiceAuthenticationProvider.MicrosoftAccount);
//
//        }
//    }
}