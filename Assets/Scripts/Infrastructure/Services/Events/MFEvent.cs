using MessagePipe;
using VContainer;

namespace MonsterFactory.Events
{
    /// <summary>
    /// Event Registration helper is just an easy way for us to keep track of events for each lifetime scope
    /// Create a new method for each lifetime scope you create.
    /// </summary>
    public static partial class EventRegistrationHelper
    {
        public static void RegisterGlobalEventClasses(IContainerBuilder builder, MessagePipeOptions options)
        {
            EventRegistrationHelper.builder = builder;
            EventRegistrationHelper.options = options;
            
            //Register Event types here
            RegisterEvent<TestEvent>();
            RegisterEvent<DataEventLoadData>();
            RegisterEvent<DataEventSaveData>();

            
            
            EventRegistrationHelper.builder = null;
            EventRegistrationHelper.options = null;
        }
    }
}

namespace MonsterFactory.Events
{

    public class MFBaseEvent
    {
    }

    public class TestEvent : MFBaseEvent
    {
    }
    
    #region DataSyatem

    public class DataEventLoadData : MFBaseEvent
    {
        
        public bool CanOverwrite { get; }
        public DataEventLoadData(bool canOverwrite)
        { 
            CanOverwrite = canOverwrite;
        }
    }

    public class DataEventSaveData : MFBaseEvent
    {
        public bool CanForceSave { get; }
        public DataEventSaveData(bool canForceSave)
        {
            CanForceSave = canForceSave;
        }
    }
    
    #endregion
}

