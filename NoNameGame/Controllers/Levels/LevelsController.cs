using System;
using System.Linq;
using GameLogic.WinPointsCalculators;
using NoNameGame.Configuration;
using NoNameGame.Configuration.InAppPurchase;
using NoNameGame.CustomControls.Levels;
using NoNameGame.Levels;
using NoNameGame.Levels.Entities;
using NoNameGame.Models;

//#if DEBUG
//using MockIAPLib;
//using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
//#endif

namespace NoNameGame.Controllers.Levels
{
    public class LevelsController
    {
        private readonly ILevelProvider _levelProvider;
        private readonly ILevelProgressStorer _levelProgressStorer;
        private readonly IWinPointsCalculator _calculator;
        private readonly LevelPackInAppPurchaseInfoProvider _appPurchaseInfoProvider;
        public LevelsController(ILevelProvider levelProvider, 
            ILevelProgressStorer levelProgressStorer,
            IWinPointsCalculator calculator,
            LevelPackInAppPurchaseInfoProvider appPurchaseInfoProvider)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (levelProgressStorer == null) throw new ArgumentNullException("levelProgressStorer");
            if (calculator == null) throw new ArgumentNullException("calculator");
            if (appPurchaseInfoProvider == null) throw new ArgumentNullException("appPurchaseInfoProvider");

            _levelProvider = levelProvider;
            _levelProgressStorer = levelProgressStorer;
            _calculator = calculator;
            _appPurchaseInfoProvider = appPurchaseInfoProvider;
        }
//        public Collection<LevelModel> GetAllLevels()
//        {
//            return new Collection<LevelModel>((from level in _levelProvider.GetAllLevels()
//                let progress = _levelProgressStorer.Load(level.Id)
//                select new LevelModel(level.Id, level.DisplayName, progress.Stars, progress.IsAvailable)).ToList());
//            
//        }
        public LevelPacksListControlModel GetLevelPacksListModel()
        {
                        
            return
                new LevelPacksListControlModel(
                    _levelProvider.GetAllLevelPacks().OrderBy(x => x.OrderNo)
                        .Select(x => new {levelPack =x,levels = _levelProvider.GetLevelGroupsForLevelPack(x.Id)
                                                                              .SelectMany(y => _levelProvider.GetLevelsForLevelGroup(y.Id))})
                        .Select(x=> new LevelPackModel(x.levelPack.Id,x.levelPack.Name,x.levelPack.Description,                            
                            x.levels.Count(),
                            x.levels.Select(y => _levelProgressStorer.Load(y.Id)).Count(y => y.IsFinished),
                            GetRomanNumber(x.levelPack.OrderNo + 1), 
                            IsLevelPackLocked(x.levelPack.Id)))
                        .ToList());
        }
        private bool IsLevelPackLocked(int levelPackId)
        {
            var levelPackInAppPurchaseInfo = _appPurchaseInfoProvider.Get(levelPackId);

            if (!levelPackInAppPurchaseInfo.IsPurchasable)
                return false;

            
            ProductLicense prodLic;
            if (CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(levelPackInAppPurchaseInfo.ProductId,
                out prodLic))
            {
                return !prodLic.IsActive;
            }
            return true;

        }
        public LevelPackControlModel GetLevelPackControlModel(int levelPackId)
        {
            LevelPackEntity levelPackEntity = _levelProvider.GetLevelPack(levelPackId);
            var lpcm =    new LevelPackControlModel(levelPackEntity.Name,
                _levelProvider.GetLevelGroupsForLevelPack(levelPackId).OrderBy(x => x.OrderNo)
                    .Select(x => new LevelGroupModel(x.Name,new SelectLevelControlModel(_levelProvider.GetLevelsForLevelGroup(x.Id)
                        .Select(y =>
                        {
                            var progress = _levelProgressStorer.Load(y.Id);
                            return new LevelModel(y.Id, y.DisplayName, progress.Stars,
                                x.AllLevelsInitiallyUnlocked || progress.IsAvailable);

                        }).ToList(), _calculator.GetMaxPossiblePoints()))).ToList());

            lpcm.LevelGroups.ForEach( 
            lg =>
            {
                lg.SelectLevelControlModel.AllLevels.First().IsAvailable = true;
                LevelModel firstUnfinished = lg.SelectLevelControlModel.AllLevels.FirstOrDefault(x => x.IsUnfinished);
                if (firstUnfinished != null)
                    firstUnfinished.PlayAnimation = true;
            });
            return lpcm;

//            return new LevelPackControlModel("Starter pack",
//                new List<LevelGroupModel>
//                {
//                    new LevelGroupModel("easy", new SelectLevelControlModel(GetAllLevels().ToList(), 3)),
//                    new LevelGroupModel("medium", new SelectLevelControlModel(GetAllLevels().ToList(), 3)),
//                    new LevelGroupModel("hard", new SelectLevelControlModel(GetAllLevels().ToList(), 3))
//                });
        }     
        private string GetRomanNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                default:
                    throw new ArgumentOutOfRangeException("number",number,"nu,ber must be less than 5 an greater than 0");
            }
        }
    }
}