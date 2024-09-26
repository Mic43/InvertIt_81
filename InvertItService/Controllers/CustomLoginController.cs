using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using InvertItService.DataObjects;
using InvertItService.Helpers;
using InvertItService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using ServiceDTOs.Login;

namespace InvertItService.Controllers
{
   

    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomLoginController : ApiController
    {
        public ApiServices Services { get; set; }
        public IServiceTokenHandler Handler { get; set; }

        // POST api/CustomLogin
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            var context = new InvertItContext();
            UserAccount account = context.UserAccounts.SingleOrDefault(a => a.Username == loginRequest.Username);
            if (account != null)
            {
                byte[] incoming = CustomLoginProviderUtils
                    .Hash(loginRequest.Password, account.Salt);

                if (CustomLoginProviderUtils.SlowEquals(incoming, account.SaltedAndHashedPassword))
                {
                    var claimsIdentity = new ClaimsIdentity();
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.Username));
                    LoginResult loginResult = new CustomLoginProvider(Handler)
                        .CreateLoginResult(claimsIdentity, Services.Settings.MasterKey);
                    var customLoginResult = new CustomLoginResult()
                    {
                        UserId = loginResult.User.UserId,
                        MobileServiceAuthenticationToken = loginResult.AuthenticationToken
                    };
                    return this.Request.CreateResponse(HttpStatusCode.OK, customLoginResult);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                "Invalid username or password");
        }

    }
}
