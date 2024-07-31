using MessagePack;

namespace MonsterFactory.Services.DataManagement
{
    [Union(0, typeof(TestData))]
    public  interface IMFData
    {
    }

    [MessagePackObject][MFDataObject("TestData", true,true)]
    public class TestData : IMFData
    {
        [Key(1)]
        public string dataString;
        
        [IgnoreMember]
        public string DataString
        {
            get => dataString;
            set => dataString = value;
        }
    }
}