using System;
using System.ComponentModel;
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
        private readonly ITypeSerializedDBService dbService;
        private readonly IDisposable eventDisposableBag;
        private readonly string typeCode;

        private T dataInstance;

        private void DataInstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!dataInstanceChanged)
            {
                dataInstanceChanged = true;
            }
        }

        private bool dataInstanceChanged;
        private readonly bool subscribeToAutoLoadEvent, subscribeToAutoSaveEvent;

        [Inject]
        public MFRuntimeDataInstanceProvider(
            ITypeSerializedDBService typeSerializedDBService,
            IAsyncSubscriber<DataEventLoadData> loadDataSubscriber,
            IAsyncSubscriber<DataEventSaveData> saveDataSubscriber)
        {
            var dataObject = MFDataSerializerExtensions.GetDataAttribute<T>();
            string uniqueId = "";
            if (dataObject == null)
            {
                DataInstance = new T();
                return;
            }

            dbService = typeSerializedDBService;
            DisposableBagBuilder disposableBagBuilder = DisposableBag.CreateBuilder();
            DataProviderTypeResolver.ResolveTypeInfo(ref uniqueId, ref subscribeToAutoLoadEvent,
                ref subscribeToAutoSaveEvent, ref typeCode, ref dataObject);
            if (subscribeToAutoLoadEvent)
            {
                loadDataSubscriber.Subscribe(LoadOrInitializeData).AddTo(disposableBagBuilder);
            }

            if (subscribeToAutoSaveEvent)
            {
                saveDataSubscriber.Subscribe(WriteChangesToDB).AddTo(disposableBagBuilder);
            }

            eventDisposableBag = disposableBagBuilder.Build();
        }

        public ref T DataInstance => ref dataInstance;


        private async UniTask LoadOrInitializeData(DataEventLoadData eventDataEventLoadData,
            CancellationToken cancellationToken)
        {
            if (DataInstance != null && !eventDataEventLoadData.CanOverwrite)
            {
                return;
            }
            dataInstance = await dbService.FetchDataFromDb<T>(typeCode, cancellationToken)
                .AttachExternalCancellation(cancellationToken);
            if (dataInstance == null)
            {
                dataInstance = new T();
                await dbService.WriteDataToDb(typeCode, cancellationToken, dataInstance);
            }
            if (dataInstance != null) dataInstance.PropertyChanged += DataInstanceOnPropertyChanged;
        }

        private async UniTask WriteChangesToDB(DataEventSaveData saveDataEvent, CancellationToken cancellationToken)
        {
            if (dataInstanceChanged || saveDataEvent.CanForceSave)
            {
                await dbService.WriteDataToDb(typeCode, cancellationToken, dataInstance);
            }
        }


        public void Dispose()
        {
            eventDisposableBag?.Dispose();
        }
    }
}