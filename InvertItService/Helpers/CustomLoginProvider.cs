using System;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;
using System.Security.Claims;

namespace InvertItService.Helpers
{
   public class CustomLoginProvider : LoginProvider
    {
        public const string ProviderName = "custom";

       public override void ConfigureMiddleware(IAppBuilder appBuilder, ServiceSettingsDictionary settings)
       {
           return;
       }
       public override ProviderCredentials CreateCredentials(ClaimsIdentity claimsIdentity)
       {
           if (claimsIdentity == null)
           {
               throw new ArgumentNullException("claimsIdentity");
           }

           string username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
           var credentials = new CustomLoginProviderCredentials
           {
               UserId = this.TokenHandler.CreateUserId(this.Name, username)
           };

           return credentials;
       }
       public override ProviderCredentials ParseCredentials(JObject serialized)
       {
           if (serialized == null)
           {
               throw new ArgumentNullException("serialized");
           }

           return serialized.ToObject<CustomLoginProviderCredentials>();
       }
       public override string Name
        {
            get { return ProviderName; }
        }

        public CustomLoginProvider(IServiceTokenHandler tokenHandler)
            : base(tokenHandler)
        {
            this.TokenLifetime = new TimeSpan(30, 0, 0, 0);
        }

    }
}