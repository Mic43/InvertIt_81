using Infrastructure.Storage;

namespace NoNameGame.Controllers.Tutorial
{
    public class AppSettingsTutorialController : ITutorialController
    {
        private const string WasTutorialShowedKeyName = "WasTutorialShowed";
        public bool ShouldShowTutorial()
        {
            return !AppSettingsAccessor.GetValueOrDefault(WasTutorialShowedKeyName, false);

        }
        public void SetTutorialShowed()
        {
            AppSettingsAccessor.AddOrUpdateValue(WasTutorialShowedKeyName,true);
            AppSettingsAccessor.Save();
        } 
    }
}