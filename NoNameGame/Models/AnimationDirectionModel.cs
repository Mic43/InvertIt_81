using System.Windows.Media;
using NoNameGame.Configuration;
using NoNameGame.Controllers.PeriodicAnimations;

namespace NoNameGame.Models
{
    public class AnimationDirectionModel
    {
        public AnimationDirectionType AnimationDirectionType { get; set; }
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAvailable
        {
            get { return !IsLocked; }
        }        
        public string UnlockConditionDescription { get; set; }

        public AnimationDirectionModel(string name, bool isLocked, string unlockConditionDescription,AnimationDirectionType animationDirectionType)
        {
            Name = name;
            IsLocked = isLocked;
            UnlockConditionDescription = unlockConditionDescription;
            AnimationDirectionType = animationDirectionType;
        }
    }
}