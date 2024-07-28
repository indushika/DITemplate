using System.IO;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public static class DataManagerDirectoryHelper
    {
        private static string UserDataPath = "MonsterFactoryUserData/SaveData";
        private static string UserDataObjectName = "UserData";
        public static string DataObjectPathForUserId(string userID)
        {
            var folderPath = $"{Application.persistentDataPath}/{UserDataPath}/{userID}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var path = $"{folderPath}/{UserDataObjectName}";
            return path;
        }
    }
}