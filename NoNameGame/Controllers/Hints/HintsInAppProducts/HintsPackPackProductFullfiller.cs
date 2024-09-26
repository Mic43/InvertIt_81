using System;
using System.Linq;
using NoNameGame.Controllers.Hints.HintsCount;

//#if DEBUG
//using MockIAPLib;
//using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
using Infrastructure.Storage;
using NoNameGame.Configuration;

//#endif



namespace NoNameGame.Controllers.Hints.HintsInAppProducts
{
              
    class HintsPackProductFullfiller : IHintsPackProductFullfiller
    {
        private readonly HintPackInAppProductCollection _hintsProductCollection;     
        private readonly IHintsCountIncreaser _hintsCountIncreaser;
        private readonly IValueStorer<string, bool> _hintPackPurchasedMarker;
        public HintsPackProductFullfiller(HintPackInAppProductCollection hintsProductCollection,
            IHintsCountIncreaser hintsCountIncreaser,IValueStorer<string,bool> hintPackPurchasedMarker)
        {
            if (hintsProductCollection == null) throw new ArgumentNullException("hintsProductCollection");            
            if (hintsCountIncreaser == null) throw new ArgumentNullException("hintsCountIncreaser");
            if (hintPackPurchasedMarker == null) throw new ArgumentNullException("hintPackPurchasedMarker");

            _hintsProductCollection = hintsProductCollection;
            _hintsCountIncreaser = hintsCountIncreaser;
            _hintPackPurchasedMarker = hintPackPurchasedMarker;
        }
        public void FullfillPendingPurchases()
        {
            var allProductLicenses = CurrentApp.LicenseInformation.ProductLicenses;
            var activeHintProducts = _hintsProductCollection.Where(hintProduct => allProductLicenses.ContainsKey(hintProduct.AppProductId))
                .ToList();

            foreach (var hintProduct in activeHintProducts)
            {
                var productLicense = allProductLicenses[hintProduct.AppProductId];

                if (!productLicense.IsConsumable)
                    throw new InvalidOperationException(string.Format("Product with id {0} must be consumable! ", hintProduct.AppProductId));

                if (productLicense.IsActive)
                {
                    CurrentApp.ReportProductFulfillment(productLicense.ProductId);

                    if (NoHintsPackPurchased())
                        MarkAsPurchased();

                    _hintsCountIncreaser.Increase(hintProduct.HintsCount);
                }
            }
        }
        private void MarkAsPurchased()
        {
            _hintPackPurchasedMarker.Write(AppSettingsKeys.IsAnyHintsPackPurchased, true);
        }
        private bool NoHintsPackPurchased()
        {
            return !_hintPackPurchasedMarker.Read(AppSettingsKeys.IsAnyHintsPackPurchased,false);
        }
    }
}