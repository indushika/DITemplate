using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Systems;
using MessagePipe;
using MonsterFactory.Services.DataManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services
{
    public class ServiceInitializer : IAsyncStartable
    {
        private readonly List<Type> lifetimeScope;
        private readonly IObjectResolver objectResolver;

        [Inject]
        public ServiceInitializer(GameLifetimeScope lifetimeScope, IObjectResolver objectResolver)
        {
            this.lifetimeScope = lifetimeScope.LifetimeServices;
            this.objectResolver = objectResolver;
        }
        
        private List<UniTask> GetInitializationTasks()
        {
            List<UniTask> initializationTasks = new List<UniTask>();
            foreach (Type type in lifetimeScope)
            {
                object instance = objectResolver.Resolve(type);
                if (instance is IMFService mfService)
                {
                    initializationTasks.AddRange(mfService.GetInitializeTasks());
                }
            }
            return initializationTasks;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            //TODO : Integrate this with loading progress
            return UniTask.WhenAll(GetInitializationTasks());
        }
    }
}