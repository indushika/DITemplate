using System;
using System.Collections.Generic;
using MessagePipe;
using MonsterFactory.Events;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using MonsterFactory.Services.Session;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Systems
{
    public class GameLifetimeScope : LifetimeScope
    {
        public List<Type> LifetimeServices;


        protected override void Configure(IContainerBuilder builder)
        {
            MFDataExtensions.Initialize();
            SetupGlobalMessageBrokers(builder);
            SetupSession(builder);
            SetupServices(builder);
            SetupDataProviders(builder);
            builder.RegisterEntryPoint<TestClass>(Lifetime.Singleton);
        }

        private void SetupDataProviders(IContainerBuilder builder)
        {
            RuntimeDataProviderRegistrationHelper.RegisterDataProviders(builder);
        }

        private void SetupServices(IContainerBuilder builder)
        {
            LifetimeServices = ServiceRegistrationHelper.RegisterServices(builder);
            builder.RegisterEntryPoint<ServiceInitializer>(); 
        }

        private void SetupSession(IContainerBuilder builder)
        {
            SessionManager.CreateSession();
        }

        private void SetupGlobalMessageBrokers(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            builder.RegisterMessageBroker<MFInternalServicesEvent>(options);
            EventRegistrationHelper.RegisterGlobalEventClasses(builder,options);
        }
    
    }
}