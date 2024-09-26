using System;
using System.Runtime.Serialization;

namespace NoNameGame.Storage
{
    [DataContract]
    public class LevelWriteableData
    {   
        [DataMember]
        public int Stars { get; set; }
        [DataMember]
        public bool IsAvailable { get; set; }
        [DataMember]
        public TimeSpan FirstSolveDuration { get; set; }

        public LevelWriteableData(int stars, bool isAvailable, TimeSpan firstSolveDuration)
        {      
            Stars = stars;
            IsAvailable = isAvailable;
            FirstSolveDuration = firstSolveDuration;
        }        
    }
}