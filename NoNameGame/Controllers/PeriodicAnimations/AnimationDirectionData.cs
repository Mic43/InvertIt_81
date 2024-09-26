using System.Runtime.Serialization;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    [DataContract]
    public class AnimationDirectionData
    {
        [DataMember]
        public bool IsLocked { get; set; }

        public AnimationDirectionData(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}