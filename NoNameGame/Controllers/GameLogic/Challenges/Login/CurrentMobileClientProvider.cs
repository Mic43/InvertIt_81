using System;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Configuration;
using NoNameGame.Helpers.Network;

namespace NoNameGame.Controllers.GameLogic.Challenges.Login
{
    public interface ICurrentMobileClientFactory
    {
        MobileServiceClient Create();
    }

    public interface ICurrentMobileClientAutheneticationProviderSetter
    {
        void Set(MobileServiceAuthenticationProvider provider);
    }

    /// <summary>
    /// SINGLETONS HERE!
    /// </summary>
    class CurrentMobileClientWithExpiryManager : ICurrentMobileClientFactory, ICurrentMobileClientAutheneticationProviderSetter
    {
        private readonly IAuthTokenProviderFactory _authenticatorFactory;
        public MobileServiceAuthenticationProvider CurrentProvider { get; private set; }
        private readonly MobileServiceClient _mobileServiceClient = null;
        private TokenExpiryHandler _authhandler;

        public CurrentMobileClientWithExpiryManager(IAuthTokenProviderFactory authenticatorFactory)
        {
            if (authenticatorFactory == null) throw new ArgumentNullException("authenticatorFactory");
            _authenticatorFactory = authenticatorFactory;
        }
        public MobileServiceClient Create()
        {
            if (_mobileServiceClient != null)
                return _mobileServiceClient;

            _authhandler = new TokenExpiryHandler(_mobileServiceClient, CurrentProvider,
                _authenticatorFactory.Create(CurrentProvider));

            var mobileServiceClient = new MobileServiceClient(Constants.InvertItServiceAddress, Constants.InvertItServiceAppKey, _authhandler);

            return mobileServiceClient;
        }
        public void Set(MobileServiceAuthenticationProvider provider)
        {
            if (provider == CurrentProvider)
                return;

            _authhandler.MobileServiceAuthenticationProvider = CurrentProvider;
            _authhandler.AuthTokenProvider = _authenticatorFactory.Create(CurrentProvider);

            CurrentProvider = provider;
            
        }
    }
}