using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NoNameGame.Storage
{
    [DataContract]
    public class LevelStorageData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<SingleMoveStorageData> MovesList { get; set; }
        [DataMember]
        public int BoardSize { get; set; }
        public int Stars { get; set; }
        public bool IsAvailable { get; set; }

        public TimeSpan FirstSolveDuration { get; set; }
        [DataMember]
        public int OrderNo { get; set; }
    }
}