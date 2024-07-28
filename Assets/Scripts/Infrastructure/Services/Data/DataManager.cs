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
        UniTask[] IMFService.GetInitializeTasks()
        {
            return new UniTask[]
            {
                InitializeDataSystems()
            };
        }


        private UniTask InitializeDataSystems()
        {
            Debug.Log("Data Systems Init");
            return default;
        }
    }
    
    
}