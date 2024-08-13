using System;
using System.Collections.Generic;
using MonsterFactory.Services.DataManagement;
using VContainer;

namespace MonsterFactory.Services
{
    /// <summary>
    /// Helper class for registering services with the container builder.
    /// </summary>
    public static class ServiceRegistrationHelper
    {
        static readonly Lifetime ServicesLifetimeType = Lifetime.Singleton;
   
        public static List<Type> RegisterServices(IContainerBuilder containerBuilder)
        {
            List<Type> servicesList = new List<Type>();
            //Add other services to be registered here
            //Any Service that requires a registered service from this list
            //should come below the register call of those classes
            RegisterService<MFLocalDBService>(containerBuilder, typeof(ITypeSerializedDBService), ref servicesList);
            RegisterService<InventoryManager>(containerBuilder, typeof(IInventoryManager), ref servicesList);
            RegisterService<BuildingManager>(containerBuilder, typeof(IBuildingManager), ref servicesList);
            RegisterService<NPCManager>(containerBuilder, typeof(INPCManager), ref servicesList);
            RegisterService<NPCAssignmentManager>(containerBuilder, typeof(INPCAssignmentManager), ref servicesList);
            
            return servicesList;
        }


        /// <summary>
        /// Registers a service with the container builder and adds the service type to the services list.
        /// </summary>
        /// <typeparam name="T">The type of the service to register. Must implement IMFService.</typeparam>
        /// <param name="containerBuilder">The container builder used for registration.</param>
        /// <param name="type">The type of the service to register.</param>
        /// <param name="servicesList">Reference to the list of registered service types.</param>
        private static void RegisterService<T>(IContainerBuilder containerBuilder, Type type, ref List<Type> servicesList) where T : IMFService
        {
            containerBuilder.Register<T>(ServicesLifetimeType).As(type);
            servicesList.Add(type);
        }

    }
}