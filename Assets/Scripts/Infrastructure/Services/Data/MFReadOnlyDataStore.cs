using System;
using Cysharp.Threading.Tasks;


namespace MonsterFactory.Services.DataManagement
{
    public class MFReadOnlyDataStore : IDisposable , IMFService
    {
        public MFReadOnlyDataStore()
        {
            
        }

        public UniTask LoadReadOnlyDataFromStorage()
        {
            return default;
        }
        
        
        public void Dispose()
        {
            // TODO release managed resources here
        }

        public UniTask[] GetInitializeTasks()
        {
            throw new NotImplementedException();
        }
    }
    
}