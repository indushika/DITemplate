using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
            RegisterService<DataManager>(containerBuilder, typeof(IDataManager), ref servicesList);
            
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

    public class TestClass : IInitializable
    {
        private readonly MFRuntimeDataInstanceProvider<TestData> testDataInstanceProvider;

        [Inject]
        public TestClass(MFRuntimeDataInstanceProvider<TestData> testDataInstanceProvider)
        {
            this.testDataInstanceProvider = testDataInstanceProvider;
        }


        public void Initialize()
        {

        }
    }
}