using System;
using System.Runtime.Serialization;

namespace NoNameGame.Levels
{
    [DataContract]
    public class LevelProgresDto
    {
        [DataMember]
        public int LevelId { get; set; }
        [DataMember]
        public int Stars { get; set; }
        [DataMember]
        public bool IsAvailable { get; set; }
        [DataMember]
        public TimeSpan FirstSolveDuration { get; set; }
    }
}