using System;
using System.IO;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public static class DataManagerDirectoryHelper
    {
        private static string UserDataPath = "MonsterFactoryUserData/SaveData";
        private static string UserDataObjectName = "UserData";
        public static string DBFilePathForUserId(string userID)
        {
            var folderPath = $"{Application.persistentDataPath}/{UserDataPath}/{userID}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var path = $"{folderPath}/{UserDataObjectName}";
            return path;
        }
        public static string StreamingDataObjectPath(string readOnlyDbName)
        {
            var folderPath = Path.Combine(Application.streamingAssetsPath, readOnlyDbName);

            if (!Directory.Exists(folderPath))
            {
                throw new Exception("Trying to read from non existent db");
            }
            
            return folderPath;
        }
    }
}