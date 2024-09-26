using Infrastructure;
using Infrastructure.Storage;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Sound
{
    public class SoundController
    {
        private const string SoundParamsKey = "SetSoundParams";
        public static readonly SoundVoume MaxSoundVoume = 0.5f;
        private const float DefaultVolume = 0.7f;

        public SetSoundParams SoundParams { get; private set; }
        public SoundController()
        {
            SetSound(AppSettingsAccessor.GetValueOrDefault(SoundParamsKey, new SetSoundParams(true,true, DefaultVolume)));            
        }
        public void SetSound(SetSoundParams setSoundParams)
        {
            SoundParams = setSoundParams;
            float volume = (float)SoundParams.Volume * MaxSoundVoume.Value;

            SoundEffect.MasterVolume = setSoundParams.IsEnabled ? volume : 0;
            MusicPlayer.SetVolume(setSoundParams.IsMusicEnabled ? volume : 0);

            AppSettingsAccessor.AddOrUpdateValue(SoundParamsKey, SoundParams);
            AppSettingsAccessor.Save();
            
        }        
    }
}