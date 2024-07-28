using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;

namespace MonsterFactory.Services
{
    public static class ServiceRegistrationHelper
    {
        static readonly Lifetime ServicesLifetimeType = Lifetime.Singleton;
   
        public static List<Type> RegisterServices(IContainerBuilder containerBuilder)
        {
            List<Type> servicesList = new List<Type>();
            RegisterService<DataManager>(containerBuilder, typeof(IDataManager), ref servicesList);
            return servicesList;
        }
        


        private static void RegisterService<T>(IContainerBuilder containerBuilder, Type type, ref List<Type> servicesList) where T : IMFService
        {
            containerBuilder.Register<T>(ServicesLifetimeType).As(type);
            servicesList.Add(type);
        }

    }
}