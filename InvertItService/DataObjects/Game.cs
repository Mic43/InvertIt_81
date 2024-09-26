using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace InvertItService.DataObjects
{
    public class Game: EntityData
    {
        public virtual UserAccount  UserAccount { get; set; }
        public string UserAccountId { get; set; }

        
        public virtual Board Board { get; set; }        
        public string BoardId;

        public int MovesCount { get; set; }

        public int Points { get; set; }
        
    }
}