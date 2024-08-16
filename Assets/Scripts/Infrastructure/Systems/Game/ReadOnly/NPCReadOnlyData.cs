using MonsterFactory.Services.DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AutoLoadDbObjects(uniqueId: "NPCReadOnlyData")]
[GenerateNativeData]
public class NPCReadOnlyData : MFData
{
    public IReadOnlyDictionary<int, List<string>> FirstNamesCollectionByLength;
    public IReadOnlyDictionary<int, List<string>> LastNamesCollectionByLength;

    public IReadOnlyDictionary<NPCAttributeType, NPCAttributeTypeData> AttributesByType;
    public IReadOnlyDictionary<NPCStatType, NPCStatTypeData> StatsByType;
}

