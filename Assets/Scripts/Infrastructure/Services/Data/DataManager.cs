using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using MonsterFactory.Services.Session;
using UnityEngine;
using VContainer;


namespace MonsterFactory.Services.DataManagement
{

    public interface IDataManager
    {
        
    }
    public class DataManager : IMFService, IDataManager
    {

        private IMFLocalDBService localDBService;


        [Inject]
        public DataManager()
        {

        }
        private UniTask InitializeDataSystems()
        {
            localDBService = new MFSqlDB(DataManagerDirectoryHelper.DataObjectPathForUserId("TestUser"));
            return localDBService.Initialize();
        }

        public UniTask[] GetInitializeTasks()
        {
            return new UniTask[]
            {
                InitializeDataSystems()
            };
        }
    }
    
    
}