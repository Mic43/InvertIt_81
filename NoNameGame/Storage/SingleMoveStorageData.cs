using System.Runtime.Serialization;

namespace NoNameGame.Storage
{
    [DataContract]
    public class SingleMoveStorageData
    {
        [DataMember]
        public int X { get; set; }
        [DataMember]
        public int Y { get; set; }
    }
}