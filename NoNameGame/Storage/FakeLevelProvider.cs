using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Infrastructure.Storage;

namespace NoNameGame.Storage
{
//    public class FakeLevelProvider : ILevelProvider
//    {
//        private List<LevelStorageData> _levelStorageData;
//
//        public FakeLevelProvider(List<LevelStorageData> levelStorageDatas)
//        {
//            _levelStorageData = levelStorageDatas;
//        }
//        public IEnumerable<LevelStorageData> GetAllLevels()
//        {
//            return _levelStorageData;
//        }
//        public LevelStorageData GetLevel(int levelId)
//        {
//            return (from l in _levelStorageData where l.Id == levelId select l).Single();
//        }
//        public void SaveLevel(LevelStorageData levelStorageData)
//        {
//            LevelStorageData storageData = _levelStorageData.Single(l => l.Id == levelStorageData.Id);
//            storageData.Stars = levelStorageData.Stars;            
//            storageData.IsAvailable = levelStorageData.IsAvailable;
//        }
//        public void SaveAllLevels()
//        {
//            IsoStorageDataManager<List<LevelStorageData>> saver = new IsoStorageDataManager<List<LevelStorageData>>();
//            saver.SaveMyData(GetAllLevels().ToList(),"Levels.bin");
//        }
//        public Maybe<LevelStorageData> GetNextLevel(int levelId)
//        {
//            var nextLevels = GetAllLevels().OrderBy(l => l.OrderNo).SkipWhile(l => l.Id != levelId);
//            return new Maybe<LevelStorageData>(nextLevels.Skip(1).FirstOrDefault());
//        }
//    }
}