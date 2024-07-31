using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace MonsterFactory.Services
{
    public interface IMFService
    {
        public UniTask[] GetInitializeTasks();
    }
}