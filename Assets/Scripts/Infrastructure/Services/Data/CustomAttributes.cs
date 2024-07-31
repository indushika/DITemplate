namespace MonsterFactory.Services.DataManagement
{
    
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)
    ]
    public class MFDataObject : System.Attribute
    {
        private readonly string uniqueId;
        private readonly bool autoFetch;
        private readonly bool autoSave;

        public MFDataObject(string uniqueId, bool autoFetch = false, bool autoSave = false)
        {
            this.uniqueId = uniqueId;
            this.autoFetch = autoFetch;
            this.autoSave = autoSave;
        }
        
        public bool AutoFetch => autoFetch;

        public string UniqueId => uniqueId;
        
        public bool AutoSave => autoSave;
    }
    
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)
    ]
    public class LocallyStoredDataObject : MFDataObject
    {
        public LocallyStoredDataObject(string uniqueId, bool autoSave = false, bool autoFetch = false) : base(uniqueId, autoFetch, true)
        {
        }
    }
}