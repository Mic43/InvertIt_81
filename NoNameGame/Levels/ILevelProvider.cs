using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using Windows.Storage.Search;
using Infrastructure;
using NoNameGame.Levels.Entities;


namespace NoNameGame.Levels
{

    // Stupid  header interface :/ poor abstraction, shame on me :(
    public interface ILevelProvider
    {
        List<LevelPackEntity> GetAllLevelPacks();
        LevelPackEntity GetLevelPack(int levelPackId);
        List<LevelGroupEntity> GetLevelGroupsForLevelPack(int levelPackId);
        List<LevelDataEntity> GetLevelsForLevelGroup(int levelGroupId);
        LevelGroupEntity GetLevelGroup(int levelGroupId);
        LevelDataEntity GetLevel(int levelId);
        List<LevelDataEntity> GetAllLevels();
        Maybe<LevelDataEntity> GetNextLevel(int levelId);
        DisabledAreasEntity GetDisabledAreas(int levelId);
    }

//    public interface ILevelFinder
//    {
//        List<LevelGroupEntity> GetLevelGroupsForLevelPack(int levelPackId);
//        List<LevelDataEntity> GetLevelsForLevelGroup(int levelGroupId);
//        LevelDataEntity GetLevel(int levelId);
//        LevelDataEntity GetNextLevel(int levelId);   
//
//    }

//    class CachingLevelPackProvider : ILevelPackProvider
//    {
//        private Dictionary<int, LevelPackEntity> _levelPackCache;
//        private Dictionary<int, LevelGroupEntity> _levelGroupCache;
//        private Dictionary<int, LevelDataEntity> _levelDataCache;
//
//
//        public ILevelPackProvider Provider { get; private set; }
//
//        public CachingLevelPackProvider(ILevelPackProvider provider)
//        {
//            Provider = provider;          
//        }
//        public void InitializeCache()
//        {
//            _levelPackCache = Provider.GetAllLevelPacks().ToDictionary(x => x.Id);
//            _levelGroupCache = Provider.
//        }
//        public List<LevelPackEntity> GetAllLevelPacks()
//        {
//            if (_levelPackCache.Keys.Any())
//                return _levelPackCache.Values.ToList();
//
//            return Provider.GetAllLevelPacks();
//        }
//        public LevelPackEntity GetLevelPack(int levelPackId)
//        {
//            if (_levelPackCache.ContainsKey(levelPackId))
//                return _levelPackCache[levelPackId];
//
//            return Provider.GetLevelPack(levelPackId);
//        }
//        public List<LevelGroupEntity> GetLevelGroupsForLevelPack(int levelPackId)
//        {
//            if (_levelGroupCache.Keys.Any())
//                return _levelPackCache.Values.ToList();
//
//            return Provider.GetAllLevelPacks();
//        }
//        public List<LevelDataEntity> GetLevelsForLevelGroup(int levelGroupId)
//        {
//            throw new System.NotImplementedException();
//        }
//        public LevelDataEntity GetLevel(int levelId)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}