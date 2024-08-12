using MessagePack;
using System.Collections.Generic;

namespace MonsterFactory.Services.DataManagement
{
    [Union(0, typeof(RuntimeGameData))]
    public  interface IMFData
    {
    }

    [MessagePackObject][MFDataObject("TestData", true,true)]
    public class TestData : IMFData
    {
        [Key(1)]
        public string dataString;
        [Key(2)]
        public List<int> npcIds = default;


        [IgnoreMember]
        public string DataString
        {
            get => dataString;
            set => dataString = value;
        }
    }
}