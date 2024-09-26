using System;
using Microsoft.WindowsAzure.MobileServices;

namespace NoNameGame.Helpers.Network
{
    public interface IMobileServiceProxy
    {
        TResult Call<TResult>(Func<IMobileServiceClient, TResult> function);
        void Call(Action<IMobileServiceClient> action);
    }

    class AuthorizeOnTokenExpiryMobileServiceProxy : IMobileServiceProxy
    {
        private readonly IMobileServiceProxy _proxy;
        public AuthorizeOnTokenExpiryMobileServiceProxy(IMobileServiceProxy proxy)
        {
            if (proxy == null) throw new ArgumentNullException("proxy");
            _proxy = proxy;
        }
        public TResult Call<TResult>(Func<IMobileServiceClient, TResult> function)
        {
            throw new NotImplementedException();
        }
        public void Call(Action<IMobileServiceClient> action)
        {
            throw new NotImplementedException();
        }
    }
}