using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Microsoft.Xna.Framework;
using NoNameGame.Levels.Converters;
using NoNameGame.Levels.DB;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels
{
    public class DbLevelProvider : ILevelProvider
    {
        private Dictionary<int, LevelPack> _levelPackCache;
        private Dictionary<int, LevelGroup> _levelGroupCache;
        private Dictionary<int, LevelData> _levelDataCache;
        private Dictionary<int, DisabledAreas> _disabledAreasCache;

        private List<LevelDataEntity> _levelDataEntityCache;

        private readonly string _connectionString;
        private LevelsContext CreateDBContext()
        {
            return new LevelsContext(_connectionString);
        }

        public List<LevelPackEntity> GetAllLevelPacks()
        {
            return _levelPackCache.Values.Select(LevelPackConverter.CreateEntity).ToList();

        }
        public LevelPackEntity GetLevelPack(int levelPackId)
        {
            return LevelPackConverter.CreateEntity(_levelPackCache[levelPackId]);

        }
        public List<LevelGroupEntity> GetLevelGroupsForLevelPack(int levelPackId)
        {
            return
                _levelGroupCache.Values.Where(x => x.LevelPackId == levelPackId)
                    .Select(LevelGroupConverter.CreateEntity)
                    .ToList();

        }
        public List<LevelDataEntity> GetLevelsForLevelGroup(int levelGroupId)
        {
            return _levelDataCache.Values.Where(x => x.LevelGroupId == levelGroupId)
                .Select(LevelDataConverter.CreateEntity)
                .OrderBy(x=>x.OrderNo)
                .ToList();
        }
        public LevelGroupEntity GetLevelGroup(int levelGroupId)
        {
            return  LevelGroupConverter.CreateEntity(_levelGroupCache[levelGroupId]);
        }
        public LevelDataEntity GetLevel(int levelId)
        {
            return LevelDataConverter.CreateEntity(_levelDataCache[levelId]);
        }
        public List<LevelDataEntity> GetAllLevels()
        {
            return _levelDataEntityCache;
        }
           
        public Maybe<LevelDataEntity> GetNextLevel(int levelId)
        {
            int levelGroupId = _levelDataCache[levelId].LevelGroupId;
            LevelGroup parentLevelGroup = _levelGroupCache.Values.Single(x => x.Id == levelGroupId);
            int orderNo =  _levelDataCache[levelId].OrderNo;
            var levelDatas = _levelDataCache.Values.Where(x=>x.LevelGroupId == levelGroupId).Where(x=>x.OrderNo > orderNo).ToList();

            if (!levelDatas.Any())
            {
                return new Maybe<LevelDataEntity>();
            }
            return new Maybe<LevelDataEntity>(LevelDataConverter.CreateEntity(levelDatas.MinBy(x=>x.OrderNo)));                    
        }
        public DisabledAreasEntity GetDisabledAreas(int levelId)
        {
            int? disabledAreasId = _levelDataCache[levelId].DisabledAreasId;
            if (!disabledAreasId.HasValue)
                return new DisabledAreasEntity(Enumerable.Empty<Point>().ToList());
            return DisabledAreasConverter.CreateEntity(_disabledAreasCache[disabledAreasId.Value]);
        }

        public DbLevelProvider(string connectionString)
        {
            _connectionString = connectionString;

            PopulateCache();
        }
        private void PopulateCache()
        {
            using (var ctx = CreateDBContext())
            {
                _levelPackCache= ctx.LevelPack.ToList().ToDictionary(x => x.Id);
                _levelGroupCache = ctx.LevelGroup.ToList().ToDictionary(x => x.Id);
                _levelDataCache = ctx.LevelData.ToList().ToDictionary(x => x.Id);
                _disabledAreasCache = ctx.DisabledAreas.ToList().ToDictionary(x => x.Id);
            }

            _levelDataEntityCache = _levelDataCache.Values.Select(LevelDataConverter.CreateEntity).ToList();
        }
    }
}