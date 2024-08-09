using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using MonsterFactory.Events;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services
{
    public static class ServiceRegistrationHelper
    {
        static readonly Lifetime ServicesLifetimeType = Lifetime.Singleton;
   
        public static List<Type> RegisterServices(IContainerBuilder containerBuilder)
        {
            List<Type> servicesList = new List<Type>();
            //Add other services to be registered here
            RegisterService<MFLocalDBService>(containerBuilder, typeof(ITypeSerializedDBService), ref servicesList);
            
            return servicesList;
        }
        


        private static void RegisterService<T>(IContainerBuilder containerBuilder, Type type, ref List<Type> servicesList) where T : IMFService
        {
            containerBuilder.Register<T>(ServicesLifetimeType).As(type);
            servicesList.Add(type);
        }

    }


    public static class RuntimeDataProviderRegistrationHelper
    {
        public static void RegisterDataProviders(IContainerBuilder containerBuilder)
        {
            containerBuilder.Register(typeof(MFRuntimeDataInstanceProvider<>), Lifetime.Singleton);
        }
    }

    public class TestClass : IAsyncStartable
    {
        private readonly MFRuntimeDataInstanceProvider<TestData> testDataInstanceProvider;
        private readonly IAsyncPublisher<DataEventLoadData> fetchEvent;
        private readonly IAsyncPublisher<DataEventSaveData> saveDataPublisher;

        [Inject]
        public TestClass(MFRuntimeDataInstanceProvider<TestData> testDataInstanceProvider, IAsyncPublisher<DataEventLoadData> fetchEvent, IAsyncPublisher<DataEventSaveData> saveDataPublisher)
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