using System.Runtime.Serialization;

namespace NoNameGame.Controllers.Sound
{
    [DataContract]
    public struct SetSoundParams
    {
        public SetSoundParams(bool isEnabled,bool isMusicEnabled, SoundVoume volume)
            : this()
        {
            IsEnabled = isEnabled;
            IsMusicEnabled = isMusicEnabled;
            Volume = volume;
        }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public bool IsMusicEnabled { get; set; }

        [DataMember]
        public SoundVoume Volume { get; set; }
    }
}