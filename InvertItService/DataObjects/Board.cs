using System.Collections.Generic;
using Microsoft.WindowsAzure.Mobile.Service;

namespace InvertItService.DataObjects
{
    public class Board :EntityData
    {
        public Board()
        {
            Games = new List<Game>();
        }
        public virtual ICollection<Game> Games { get; set; }
        public int Size { get; set; }
        public int MovesCount { get; set; }
        public string MovesCollection { get; set; }
    }
}