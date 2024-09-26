using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NoNameGame.Controllers.Hints.HintsInAppProducts;
using NoNameGame.CustomControls;

//#if DEBUG
//using MockIAPLib;
//using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
using NoNameGame.Controllers.Hints.HintsCount;

//
//#endif


namespace NoNameGame.Controllers.Hints
{
    public class HintsPurchaseController
    {
        private readonly HintPackInAppProductCollection _hintPackInAppProductCollection;
        private readonly IHintsCountProvider _hintsCountProvider;
        private readonly IHintsPackProductFullfiller _hintsPackProductFullfiller;
        public HintsPurchaseController(HintPackInAppProductCollection hintPackInAppProductCollection,IHintsCountProvider hintsCountProvider,
            IHintsPackProductFullfiller hintsPackProductFullfiller)
        {
            if (hintPackInAppProductCollection == null)
                throw new ArgumentNullException("hintPackInAppProductCollection");
            if (hintsCountProvider == null) throw new ArgumentNullException("hintsCountProvider");
            if (hintsPackProductFullfiller == null)
                throw new ArgumentNullException("hintsPackProductFullfiller");

            _hintPackInAppProductCollection = hintPackInAppProductCollection;
            _hintsCountProvider = hintsCountProvider;
            _hintsPackProductFullfiller = hintsPackProductFullfiller;
        }


        public async void PurchaseHintPack(string productId)
        {
            if (productId == null) throw new ArgumentNullException("productId");
            if (!_hintPackInAppProductCollection.Contains(productId))
                throw new ArgumentException("Provided product id is not present in registered hint packs in HintPackInAppProductCollection");
         
            try
            {
                await CurrentApp.RequestProductPurchaseAsync(productId, false);   
                _hintsPackProductFullfiller.FullfillPendingPurchases();
            }
            catch (Exception)
            {                
                // MessageBox.Show(e.ToString());
            }   
        }
        public async Task<List<PurchaseHintsModel>> GetHintsPack()
        {      
            //Thread.Sleep(1000);
            try
            {
                var res = await CurrentApp.LoadListingInformationByProductIdsAsync(
                    _hintPackInAppProductCollection.Select(x => x.AppProductId).ToArray());

               var purchaseHintsModels = res.ProductListings.Values                    
                    .OrderBy(x=>int.Parse(x.Tag))                    
                    .Select(x =>
                        new PurchaseHintsModel()
                        {
                            Description = x.Description,
                            Name = x.Name,
                            Price = x.FormattedPrice,
                            ProductId = x.ProductId
                        }).ToList();                
                
                purchaseHintsModels.Last().IsBestDeal = true;
                return purchaseHintsModels;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot contact with Microsoft App Purchase servers at the moment. Try again later!");
                return Enumerable.Empty<PurchaseHintsModel>().ToList();
            }                  
        }

        public void FullfillPendingPurchases()
        {
            _hintsPackProductFullfiller.FullfillPendingPurchases();
        }
        public int GetCurrentHintsCount()
        {
           return _hintsCountProvider.Get();
        }

    }
}