using System;

namespace ServiceDTOs.Login
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