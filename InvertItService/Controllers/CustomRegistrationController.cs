using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using InvertItService.DataObjects;
using InvertItService.Helpers;
using InvertItService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using ServiceDTOs.Login;

namespace InvertItService.Controllers
{
 //     [AuthorizeLevel(AuthorizationLevel.Application)]
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomRegistrationController : ApiController
    {
        public ApiServices Services { get; set; }

        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {          

            if (!Regex.IsMatch(registrationRequest.Username, "^[a-zA-Z0-9]{4,}$"))
            {
                return this.Request.CreateResponse(HttpStatusCode.Created, RegistrationResponse.CreateFailure(RegistrationResult.InvalidUsername));
            }
            else if (registrationRequest.Password.Length < 8)
            {
                return this.Request.CreateResponse(HttpStatusCode.Created, RegistrationResponse.CreateFailure(RegistrationResult.InvalidPassword));
            }

            var context = new InvertItContext();
            UserAccount account = context.UserAccounts.SingleOrDefault(a => a.Username == registrationRequest.Username);
            if (account != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.Created, RegistrationResponse.CreateFailure(RegistrationResult.UserAlreadyExists));
            }
            else
            {
                byte[] salt = CustomLoginProviderUtils.GenerateSalt();
                var newAccount = new UserAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = registrationRequest.Username,
                    Salt = salt,
                    SaltedAndHashedPassword = CustomLoginProviderUtils.Hash(registrationRequest.Password, salt)                    
                };
                context.UserAccounts.Add(newAccount);
                context.UserRankings.Add(new UserRanking() {UserAccount = newAccount});
                context.SaveChanges();

                return this.Request.CreateResponse(HttpStatusCode.Created, RegistrationResponse.CreateSuccess());
            }
        }
    }    
   
}
