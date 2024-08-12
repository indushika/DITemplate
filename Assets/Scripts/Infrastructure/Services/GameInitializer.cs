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
    public class GameInitializer : IAsyncStartable
    {
        private readonly MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider;
        private readonly IAsyncPublisher<FetchSaveData> fetchEvent;

        [Inject]
        public GameInitializer(MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider, IAsyncPublisher<FetchSaveData> fetchEvent)
        {
            this.runtimeDataInstanceProvider = runtimeDataInstanceProvider;
            this.fetchEvent = fetchEvent;
        }

        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await fetchEvent.PublishAsync(new FetchSaveData(true), cancellation);
            //Debug.Log(testDataInstanceProvider.DataInstance.DataString);
        }
    }
}