using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReadOnlyGameData
{
    public readonly IReadOnlyDictionary<int, List<string>> FirstNamesCollectionByLength;
    public readonly IReadOnlyDictionary<int, List<string>> LastNamesCollectionByLength;

    public readonly IReadOnlyDictionary<NPCAttributeType, NPCAttributeData> AttributesByType;
    public readonly IReadOnlyDictionary<NPCStatType,  NPCStatData> StatsByType;

    public readonly IReadOnlyDictionary<AssignmentTypeId, AssignmentTypeData> AssignmentTypeDataById;

    public readonly IReadOnlyDictionary<ResourceTypeId, ResourceTypeData> ResourceTypeDataById;

    public readonly IReadOnlyDictionary<BuildingTypeId, BuildingTypeData> BuildingTypeDataById;

}
