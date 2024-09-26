namespace NoNameGame.Controllers.Vibrator
{
    public abstract class PhoneVibrator
    {
        private static readonly object padlock = new object();
        private static PhoneVibrator _current;
        public static PhoneVibrator Current
        {
            get
            {
                lock (padlock)
                {
                    if (_current == null)
                        _current = new NullPhoneVibrator();
                    return _current;
                }
            }
            set
            {
                lock (padlock)
                {
                    _current = value;    
                }                
            }            
        }

        public abstract void Vibrate();
    }
}