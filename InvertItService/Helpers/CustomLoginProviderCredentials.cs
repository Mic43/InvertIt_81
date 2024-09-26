using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace InvertItService.Helpers
{
    public class CustomLoginProviderCredentials : ProviderCredentials
    {
        public CustomLoginProviderCredentials()
            : base(CustomLoginProvider.ProviderName)
        {
        }
    }
}