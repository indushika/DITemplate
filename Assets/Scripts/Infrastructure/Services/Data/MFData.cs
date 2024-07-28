using System.ComponentModel;
using System.Threading;
using MessagePack;
using MonsterFactory.Services.DataManagement;
using VContainer.Unity;

namespace MonsterFactory.Services.DataManagement
{
    [MessagePackObject]
    public abstract class MFData
    {
        internal string InstanceId;
    }
}