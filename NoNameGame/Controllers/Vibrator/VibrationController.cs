using Infrastructure.Storage;

namespace NoNameGame.Controllers.Vibrator
{
    public class VibrationController
    {
         private const string VibrationsParamsKey = "VibrationsParams";
         public VibrationParams VibrationParams { get; private set; }
        public VibrationController()
        {
            SetVibration(AppSettingsAccessor.GetValueOrDefault(VibrationsParamsKey, new VibrationParams(true)));            
        }
        public void SetVibration(VibrationParams vibrationParams)
        {
            VibrationParams = vibrationParams;

            if (vibrationParams.IsEnabled)
                PhoneVibrator.Current = new ShortPhoneVibrator();
            else
                PhoneVibrator.Current = new NullPhoneVibrator();

            AppSettingsAccessor.AddOrUpdateValue(VibrationsParamsKey, VibrationParams);
            AppSettingsAccessor.Save();
            
        }         
    }
}