using System.Runtime.Serialization;

namespace GameLogic
{
    [DataContract]
    public enum GameState
    {
        [EnumMember]
        Started = 0,
        [EnumMember]
        Won = 1
    }
}