﻿using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services
{
    public class TestClass : IAsyncStartable
    {
        private readonly MFLocallyStoredDataInstanceProvider<TestData> testDataInstanceProvider;
        private readonly IAsyncPublisher<DataEventLoadData> fetchEvent;
        private readonly IAsyncPublisher<DataEventSaveData> saveDataPublisher;

        [Inject]
        public TestClass(MFLocallyStoredDataInstanceProvider<TestData> testDataInstanceProvider, IAsyncPublisher<DataEventLoadData> fetchEvent, IAsyncPublisher<DataEventSaveData> saveDataPublisher)
        {
            this.testDataInstanceProvider = testDataInstanceProvider;
            this.fetchEvent = fetchEvent;
            this.saveDataPublisher = saveDataPublisher;
        }
        

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await fetchEvent.PublishAsync(new DataEventLoadData(true), cancellation);
            //testDataInstanceProvider.DataInstance.DataString = "TEST";
            //await saveDataPublisher.PublishAsync(new DataEventSaveData(false));
            Debug.Log(testDataInstanceProvider.DataInstance.DataString);
        }
    }
}