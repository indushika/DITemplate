using MonsterFactory.Services.DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[AutoLoadDbObjects(uniqueId: "ReadOnlyGameData")]
public class ReadOnlyGameData : MFData
{
    public IReadOnlyDictionary<int, List<string>> FirstNamesCollectionByLength;
    public IReadOnlyDictionary<int, List<string>> LastNamesCollectionByLength;

    public IReadOnlyDictionary<NPCAttributeType, NPCAttributeData> AttributesByType;
    public IReadOnlyDictionary<NPCStatType,  NPCStatData> StatsByType;

    public IReadOnlyDictionary<AssignmentTypeId, AssignmentTypeData> AssignmentTypeDataById;

    public IReadOnlyDictionary<ResourceTypeId, ResourceTypeData> ResourceTypeDataById;

    public IReadOnlyDictionary<BuildingTypeId, BuildingTypeData> BuildingTypeDataById;

}
