using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace Infrastructure
{
    public class IsoDataSaver<TMyDataType>
    {
        private const string TargetFolderName = "SaveGame";
        private DataContractSerializer _mySerializer;
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

        public IsoDataSaver()
        {
            _mySerializer = new DataContractSerializer(typeof(TMyDataType));
        }

        public void SaveMyData(TMyDataType sourceData, String targetFileName)
        {
            string TargetFileName = String.Format("{0}\\{1}.dat",
                                           TargetFolderName, targetFileName);
            if (!IsoFile.DirectoryExists(TargetFolderName))
                IsoFile.CreateDirectory(TargetFolderName);
         
            using (var targetFile = IsoFile.OpenFile(TargetFileName,FileMode.OpenOrCreate))
            {
                _mySerializer.WriteObject(targetFile, sourceData);
            }                   
        }

        public TMyDataType LoadMyData(string sourceName)
        {
            TMyDataType retVal = default(TMyDataType);
            string TargetFileName = String.Format("{0}\\{1}.dat",
                                                  TargetFolderName, sourceName);
            if (IsoFile.FileExists(TargetFileName))
                using (var sourceStream =
                        IsoFile.OpenFile(TargetFileName, FileMode.Open))
                {
                    retVal = (TMyDataType)_mySerializer.ReadObject(sourceStream);
                }
            return retVal;
        }
    }
}