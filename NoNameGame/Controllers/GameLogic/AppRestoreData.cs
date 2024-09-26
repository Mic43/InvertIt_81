using System.Runtime.Serialization;
using GameLogic.Game;
using NoNameGame.Models;

namespace NoNameGame.Controllers.GameLogic
{
    [DataContract]
    public class AppRestoreData
    {
        [DataMember]
        public GameData GameData { get; set; }
        [DataMember]
        public GameWonModel GameWonModel { get; set; }
    }
}