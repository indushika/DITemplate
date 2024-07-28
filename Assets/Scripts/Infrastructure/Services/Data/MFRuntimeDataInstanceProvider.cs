using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;

namespace MonsterFactory.Services.DataManagement
{
    public class MFRuntimeDataInstanceProvider<T> : IDisposable where T : MFData, new()
    {
        private readonly IMFLocalDBService dbService;
        private IDisposable eventDisposableBag;
        protected readonly string TypeCode;
        protected int dataInstanceId = -1;
        protected bool autoSave;
        private T dataInstance;
        private MFDataObject dataObject;

        [Inject]
        public MFRuntimeDataInstanceProvider(IMFLocalDBService dbService, IAsyncSubscriber<FetchSaveData> autoFetchSubscriber)
        {
            dataObject = GetDataAttribute();
            string uniqueId = "";
            if (dataObject == null)
            {
                dataInstance = new T();
                return;
            }
            this.dbService = dbService;
            DisposableBagBuilder disposableBagBuilder = DisposableBag.CreateBuilder();
            DataProviderTypeResolver.ResolveTypeInfo(ref uniqueId, ref autoSave, ref TypeCode, ref dataObject);
            if (autoSave)
            {
                autoFetchSubscriber.Subscribe((InitializeDataObject)).AddTo(disposableBagBuilder);
            }
            eventDisposableBag = disposableBagBuilder.Build();
        }

        public T DataInstance
        {
            get => dataInstance;
            set => UpdateDataInstance(value);
        }

        public T UpdateDataInstance(T value)
        {
            return dataInstance = value;
        }

        private async UniTask InitializeDataObject(FetchSaveData eventData, CancellationToken cancellationToken)
        {
            if (dataInstance != null && !eventData.OverwriteExistingData)
            {
                return;
            }
            bool fetchState = await FetchDataFromDb(cancellationToken).AttachExternalCancellation(cancellationToken);
            if (!fetchState)
            {
                dataInstance = new T();
                var dataChunk = new DataChunk();
                await dbService.AddNewDataInstance(dataChunk);
                dbService.AddDataChunkMap(TypeCode, dataChunk.DataChunkId);
            }
        }


        private async UniTask<bool> FetchDataFromDb(CancellationToken cancellationToken)
        {
            try
            {
                DataChunkMap dataChunkMap = await dbService.GetChunkUniqueDataFromKey(TypeCode).AttachExternalCancellation(cancellationToken);
                if (dataChunkMap != null)
                {
                    dataInstanceId = dataChunkMap.DataChunkId;
                    return await TryProcessDataChunk().AttachExternalCancellation(cancellationToken);
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

        public void Dispose()
        {
            eventDisposableBag?.Dispose();
        }
    }
}