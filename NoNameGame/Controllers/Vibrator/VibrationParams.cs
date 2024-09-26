using System.Runtime.Serialization;

namespace NoNameGame.Controllers.Vibrator
{
    [DataContract]

    public class VibrationParams
    {
        [DataMember]
        public bool IsEnabled { get; set; }
        public VibrationParams(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}