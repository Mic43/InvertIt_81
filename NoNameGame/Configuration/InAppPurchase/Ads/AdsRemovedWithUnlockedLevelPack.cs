using System;

//#if DEBUG
//using MockIAPLib;
//using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
//#endif


namespace NoNameGame.Configuration.InAppPurchase.Ads
{
    class AdsRemovedWithUnlockedLevelPack : IAdsRemovalProvider
    {
        private readonly LevelPackInAppPurchaseInfoProvider _appPurchaseInfoProvider;
        private readonly int _levelPackId;
        public AdsRemovedWithUnlockedLevelPack(LevelPackInAppPurchaseInfoProvider appPurchaseInfoProvider, int levelPackId)
        {
            if (appPurchaseInfoProvider == null) throw new ArgumentNullException("appPurchaseInfoProvider");
            _appPurchaseInfoProvider = appPurchaseInfoProvider;
            _levelPackId = levelPackId;
        }
        public bool AreRemoved()
        {
            ProductLicense prodLic;
            return CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(
                _appPurchaseInfoProvider.Get(_levelPackId).ProductId, out prodLic) && prodLic.IsActive;
        }
    }
}