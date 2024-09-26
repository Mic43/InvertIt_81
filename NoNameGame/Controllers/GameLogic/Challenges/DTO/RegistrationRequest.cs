using System;

namespace NoNameGame.Controllers.GameLogic.Challenges.DTO
{
    public class RegistrationRequest
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public RegistrationRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}