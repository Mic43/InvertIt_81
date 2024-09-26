using System;
using System.Runtime.Serialization;

namespace NoNameGame.Controllers.Sound
{
    [DataContract]
    public struct SoundVoume
    {

        private float _value;
        public SoundVoume(float value) : this()
        {
            if(value < 0.0 || value > 1.0)
                throw new ArgumentException("value","value must fall between 0.0 and 1.0");
            Value = value;
        }
        [DataMember]
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public static explicit operator float(SoundVoume volume)  // implicit digit to byte conversion operator
        {
            return volume.Value; // implicit conversion
        }
        public static implicit operator SoundVoume(float value)
        {
            return new SoundVoume(value);
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}