using System.Runtime.Serialization;

namespace NoNameGame.Controllers.Themes
{
    [DataContract]
    public class ThemeData
    {
        [DataMember]
        public bool IsLocked { get; set; }
        public ThemeData(bool isLocked)
        {
            IsLocked = isLocked;            
        }
    }
}