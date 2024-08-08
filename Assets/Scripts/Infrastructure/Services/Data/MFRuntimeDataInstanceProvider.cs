using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using UnityEngine;
using VContainer;

namespace MonsterFactory.Services.DataManagement
{
    public class MFRuntimeDataInstanceProvider<T> : IDisposable where T : MFData, new()
    {
        private readonly IMFLocalDBService dbService;
        private IDisposable eventDisposableBag;
        protected readonly string TypeCode;
        protected readonly bool autoFetch;
        private T dataInstance;
        private MFDataObject dataObject;

        [Inject]
        public MFRuntimeDataInstanceProvider(IDataManager dataManager,
            IAsyncSubscriber<FetchSaveData> autoFetchSubscriber)
        {
            dataObject = GetDataAttribute();
            string uniqueId = "";
            if (dataObject == null)
            {
                dataInstance = new T();
                return;
            }

            dbService = dataManager.LocalDBService();
            DisposableBagBuilder disposableBagBuilder = DisposableBag.CreateBuilder();
            DataProviderTypeResolver.ResolveTypeInfo(ref uniqueId, ref autoFetch, ref TypeCode, ref dataObject);
            if (autoFetch)
            {
                autoFetchSubscriber.Subscribe(InitializeDataObject).AddTo(disposableBagBuilder);
            }

            eventDisposableBag = disposableBagBuilder.Build();
        }

        public ref T DataInstance => ref dataInstance;

        public async UniTask UpdateDataInstance()
        {
            await dbService.WriteDataChunkToId(TypeCode, dataInstance?.SerializeDataToBytes());
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
                dbService.AddDataChunkMap(TypeCode);
            }
        }


        private async UniTask<bool> FetchDataFromDb(CancellationToken cancellationToken)
        {
            try
            {
                DataChunkMap dataChunkMap = await dbService.GetChunkUniqueDataFromKey(TypeCode)
                    .AttachExternalCancellation(cancellationToken);
                if (dataChunkMap != null)
                {
                    return await TryProcessDataChunk().AttachExternalCancellation(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"DB Fetch {TypeCode} Unknown Error : {e}");
                return false;
            }

            return false;
        }

        private async UniTask<bool> TryProcessDataChunk()
        {
            DataChunkMap dataChunk = await dbService.GetDataChunkById(TypeCode);
            MFData var = dataChunk.ExtractDataObjectOfType<T>();
            if (var is T data)
            {
                dataInstance = data;
            }

            return DataInstance != null;
        }

        private static MFDataObject GetDataAttribute()
        {
            object[] attributes = typeof(T).GetCustomAttributes(typeof(MFDataObject), true);
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