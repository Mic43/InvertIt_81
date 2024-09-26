using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace Infrastructure.Storage
{
    public class IsoStorageDataManager<TMyDataType>
    {        
        private readonly DataContractSerializer _mySerializer;
        private IsolatedStorageFile _isoFile;
        IsolatedStorageFile IsoFile
        {
            get
            {
                if (_isoFile == null)
                    _isoFile = System.IO.IsolatedStorage.
                                IsolatedStorageFile.GetUserStoreForApplication();
                return _isoFile;
            }
        }

        public IsoStorageDataManager()
        {
            _mySerializer = new DataContractSerializer(typeof(TMyDataType));
        }

        public void SaveMyData(TMyDataType sourceData, String targetFileName)
        {
//            string TargetFileName = String.Format("{0}\\{1}.dat",
//                                           TargetFolderName, targetFileName);
//            if (!IsoFile.DirectoryExists(TargetFolderName))
//                IsoFile.CreateDirectory(TargetFolderName);
//         
            using (var targetFile = IsoFile.OpenFile(targetFileName, FileMode.OpenOrCreate))
            {
                _mySerializer.WriteObject(targetFile, sourceData);
            }                   
        }

        public TMyDataType LoadMyData(string targetFileName)
        {
            TMyDataType retVal = default(TMyDataType);         
           
            using (var sourceStream =
                    IsoFile.OpenFile(targetFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                retVal = (TMyDataType)_mySerializer.ReadObject(sourceStream);
            }
            return retVal;
        }
    }
}