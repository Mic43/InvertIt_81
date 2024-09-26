using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Infrastructure;
using Infrastructure.Storage;

namespace NoNameGame.Storage
{
    //Stupid naive implementation
//    public class ManifestResourceLevelProvider : ILevelProvider
//    {
//
//        private IsoStorageDataManager<List<LevelStorageData>> _isoStorageDataManager = new IsoStorageDataManager<List<LevelStorageData>>();        
//        private string _levelResourceFileName;
//        public ManifestResourceLevelProvider(string levelResourceFileName)
//        {
//            _levelResourceFileName = levelResourceFileName;
//            
//        }
//
//        public IEnumerable<LevelStorageData> GetAllLevels()
//        {
//
//            var assembly = Assembly.GetExecutingAssembly();          
//
//            using (Stream stream = assembly.GetManifestResourceStream(_levelResourceFileName))
//            {
//                var mySerializer = new DataContractSerializer(typeof (List<LevelStorageData>));
//                var levelStorageDatas = (List<LevelStorageData>) mySerializer.ReadObject(stream);
//
//                var storageDatas = from lsd in levelStorageDatas
//                let writeableData= SettingsAccessor.GetValueOrDefault(lsd.Id.ToString(),new LevelWriteableData(0,lsd.OrderNo== 0,TimeSpan.Zero))
//                select new LevelStorageData()
//                {
//                    BoardSize = lsd.BoardSize,
//                    FirstSolveDuration = writeableData.FirstSolveDuration,
//                    Id = lsd.Id,
//                    IsAvailable = writeableData.IsAvailable,
//                    MovesList = lsd.MovesList,
//                    OrderNo = lsd.OrderNo,
//                    Stars = writeableData.Stars
//                };
//                return storageDatas;
//            }
//        }
//
//        public LevelStorageData GetLevel(int levelId)
//        {
//            return GetAllLevels().Single(l => l.Id == levelId);
//        }
//        public Maybe<LevelStorageData> GetNextLevel(int levelId)
//        {
//            var nextLevels = GetAllLevels().OrderBy(l => l.OrderNo).SkipWhile(l => l.Id != levelId);
//            return new Maybe<LevelStorageData>(nextLevels.Skip(1).FirstOrDefault());
//        }
//        public void SaveLevel(LevelStorageData levelStorageData)
//        {
//            SettingsAccessor.AddOrUpdateValue(levelStorageData.Id.ToString(),
//                new LevelWriteableData(levelStorageData.Stars, levelStorageData.IsAvailable,
//                    levelStorageData.FirstSolveDuration));
//        }
//        public void SaveAllLevels()
//        {
//            throw new NotImplementedException();
//        }
//    }

}