using System;

namespace NoNameGame.Configuration.InAppPurchase
{
    public class LevelPackInAppPurchaseInfoProvider
    {
        private readonly LevelPackInAppPurchaseInfoDictionary _appPurchaseInfoDictionary = new LevelPackInAppPurchaseInfoDictionary
        {
            new LevelPackInAppPurchaseInfo(1,null),
            new LevelPackInAppPurchaseInfo(7,null),
            new LevelPackInAppPurchaseInfo(9,null),
            new LevelPackInAppPurchaseInfo(12,"9x9UltraLevelPack")
//            new LevelPackInAppPurchaseInfo(12,"TestLevelPack")
           
        };     
        public LevelPackInAppPurchaseInfo Get(int levelPackId)
        {
            if (!_appPurchaseInfoDictionary.Contains(levelPackId))            
                throw new ArgumentException("Provided level pack is not present in inner dictionary. " +
                                            "Wrong value was given or you should add it to the inner dictionary. " +
                                            "_appPurchaseInfoDictionary must contain all valid levels packs","levelPackId");
            return _appPurchaseInfoDictionary[levelPackId];
        }
    }
}