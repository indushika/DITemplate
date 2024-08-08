using System;
using MessagePack;
namespace MonsterFactory.Services.DataManagement
{
    [Union(0, typeof(TestData))]
    public abstract class MFData
    {
    }

    [MessagePackObject][MFDataObject("TestData", true,true)]
    public class TestData : MFData
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