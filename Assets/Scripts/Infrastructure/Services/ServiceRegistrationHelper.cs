using System;
using System.Collections.Generic;
using MonsterFactory.Services.DataManagement;
using VContainer;

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
            RegisterService<InventoryManager>(containerBuilder, typeof(IInventoryManager), ref servicesList);
            RegisterService<BuildingManager>(containerBuilder, typeof(IBuildingManager), ref servicesList);
            RegisterService<NPCManager>(containerBuilder, typeof(INPCManager), ref servicesList);
            RegisterService<NPCAssignmentManager>(containerBuilder, typeof (INPCAssignmentManager), ref servicesList);
            
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
}