using System;

namespace ServiceDTOs.Login
{
    public class LoginRequest
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}