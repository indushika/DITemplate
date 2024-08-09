using System;
using System.Collections.Generic;
using System.ComponentModel;
using MessagePack;
using Unity.VisualScripting;

namespace MonsterFactory.Services.DataManagement
{
    [Union(0, typeof(TestData))]
    public abstract class MFData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected bool SetField<T>(ref T field, T value)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(null, null);
            return true;
        }
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
            set =>  SetField(ref dataString , value);
        }
    }


    [MessagePackObject][MFDataObject("TestData" , true,true)]
    public readonly struct IndexedReadOnlyData<T> where T : MFData
    {
        public readonly IReadOnlyDictionary<int, T> ReadOnlyDataDictionary;
    }


    
    public class NPCStatsTypeData : MFData
    {
        public string Description;
        public string ToolTipText;
    }
}