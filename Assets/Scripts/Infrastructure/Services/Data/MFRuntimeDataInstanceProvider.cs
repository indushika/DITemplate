using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;

namespace MonsterFactory.Services.DataManagement
{
    public abstract class MFRuntimeDataInstanceProvider<T> where T : MFData, new()
    {
        private readonly IMFLocalDBService dbService;
        protected readonly string TypeCode;
        protected int dataInstanceId = -1;
        protected bool autoSave;
        private T dataInstance;
        private MFDataObject dataObject;

        [Inject]
        protected MFRuntimeDataInstanceProvider(IMFLocalDBService dbService)
        {
            dataObject = GetDataAttribute();
            string uniqueId = "";
            if (dataObject == null)
            {
                dataInstance = new T();
                return;
            }

            this.dbService = dbService;
            uniqueId = dataObject.UniqueId;
            autoSave = dataObject.AutoSave;
            if (!string.IsNullOrEmpty(uniqueId))
            {
                var hashedBytes = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(uniqueId));
                string hashedString = Convert.ToBase64String(hashedBytes);
                TypeCode = hashedString.Substring(0, 64);
            }
        }

        protected T DataInstance
        {
            get => dataInstance;
            set => UpdateDataInstance(value);
        }

        public T UpdateDataInstance(T value)
        {
            return dataInstance = value;
        }

        protected async void InitializeDataObject()
        {
            bool fetchState = await FetchDataFromDb();
            if (!fetchState)
            {
                dataInstance = new T();
                var dataChunk = new DataChunk();
                await dbService.AddNewDataInstance(dataChunk);
                dbService.AddDataChunkMap(TypeCode, dataChunk.DataChunkId);
            }
        }


        private async UniTask<bool> FetchDataFromDb()
        {
            try
            {
                DataChunkMap dataChunkMap = await dbService.GetChunkUniqueDataFromKey(TypeCode);
                if (dataChunkMap != null)
                {
                    dataInstanceId = dataChunkMap.DataChunkId;
                    return await TryProcessDataChunk();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"DB Fetch {TypeCode} Unknown Error");
                return false;
            }

            return false;
        }

        private async UniTask<bool> TryProcessDataChunk()
        {
            DataChunk dataChunk = await dbService.GetDataChunkById(dataInstanceId);
            DataInstance = dataChunk.ExtractDataObjectOfType<T>();
            return DataInstance != null;
        }

        private static MFDataObject GetDataAttribute()
        {
            object[] attributes = typeof(T).GetCustomAttributes(typeof(T), true);
            foreach (var attribute in attributes)
            {
                if (attribute is MFDataObject dataObjectAttribute)
                {
                    return dataObjectAttribute;
                }
            }

            return null;
        }
    }
}