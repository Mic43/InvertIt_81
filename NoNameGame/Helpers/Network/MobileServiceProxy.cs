using System;
using Microsoft.WindowsAzure.MobileServices;

namespace NoNameGame.Helpers.Network
{
    public class MobileServiceProxy : IMobileServiceProxy
    {
        private readonly IMobileServiceClient _mobileService;
        public MobileServiceProxy(IMobileServiceClient mobileService)
        {
            if (mobileService == null) throw new ArgumentNullException("mobileService");
            _mobileService = mobileService;
        }
        public TResult Call<TResult>(Func<IMobileServiceClient, TResult> function)
        {
            if (function == null) throw new ArgumentNullException("function");

            return function.Invoke(_mobileService);
        }
        public  void Call(Action<IMobileServiceClient> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            
            
            Call(client =>
            {
                action(client);
                return 0;
            });
        }
    }
}