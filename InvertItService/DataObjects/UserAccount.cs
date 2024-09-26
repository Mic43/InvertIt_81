using System.Collections.Generic;
using InvertItService.Logic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace InvertItService.DataObjects
{
    public class UserAccount : EntityData
    {    
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }


        public UserAccount()
        {
            Games = new List<Game>();
        }

        public virtual ICollection<Game> Games { get; set; }
    }
}