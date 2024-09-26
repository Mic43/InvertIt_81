using Infrastructure.Storage;

namespace NoNameGame.Controllers
{
    public class NewItemUnlockedStorer
    {
        private const string IsNewItemUnlockedKey = "IsNewItemUnlocked";
       
        public void SetNewItemUnlocked()
        {
            AppSettingsAccessor.AddOrUpdateValue(IsNewItemUnlockedKey,true);
            AppSettingsAccessor.Save();
        }
        public void ClearNewItemUnlocked()
        {
            AppSettingsAccessor.AddOrUpdateValue(IsNewItemUnlockedKey, false);
            AppSettingsAccessor.Save();
        }
        public bool IsNewItemUnlocked()
        {
           return  AppSettingsAccessor.GetValueOrDefault(IsNewItemUnlockedKey, false);
        }

    }
}