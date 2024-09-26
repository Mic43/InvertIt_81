using System.Collections.ObjectModel;

namespace NoNameGame.Configuration.InAppPurchase
{
    public class LevelPackInAppPurchaseInfoDictionary : KeyedCollection<int, LevelPackInAppPurchaseInfo>
    {
        protected override int GetKeyForItem(LevelPackInAppPurchaseInfo item)
        {
            return item.LevelPackId;
        }
    }
}